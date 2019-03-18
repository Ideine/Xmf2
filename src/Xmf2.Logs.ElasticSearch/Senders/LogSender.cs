using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xmf2.Core.Extensions;
using Xmf2.Core.Workers;
using Xmf2.Logs.ElasticSearch.Interfaces;
using Xmf2.Logs.ElasticSearch.Workers;

namespace Xmf2.Logs.ElasticSearch.Senders
{
	internal class LogSender : ILogSender
	{
		private readonly HttpClient _client;
		private readonly string _url;
		private readonly ILogBufferStorage _storage;
		private readonly BackgroundWorker _worker;

		private readonly StringBuilder _waitingContent = new StringBuilder();
		private readonly SemaphoreSlim _stringBuilderMutex = new SemaphoreSlim(1);
		private readonly SemaphoreSlim _contentSemaphore = new SemaphoreSlim(0, 100000000);
		private readonly SemaphoreSlim _runMutex = new SemaphoreSlim(1);

		private readonly ExponentialBackOffStrategy _backOffStrategy;

		public LogSender(HttpClient client, string url, ILogBufferStorage storage)
		{
			_client = client;
			_url = url;
			_storage = storage;

			_backOffStrategy = new ExponentialBackOffStrategy(5000, 4);

			string storedContent = _storage.Load();
			if (!string.IsNullOrEmpty(storedContent))
			{
				_waitingContent.Append(storedContent);
				_contentSemaphore.Release();
			}

			_worker = new BackgroundWorker(SendAnyPendingContentAsync);
			_worker.Start();
		}

		public async void Enqueue(ILogEntry entry)
		{
			using (await _stringBuilderMutex.LockAsync())
			{
				if (_waitingContent.Length == 0)
				{
					_contentSemaphore.Release();
				}
				_waitingContent.AppendLine($"{{\"index\":{{\"_index\":\"{entry.Index}\", \"_type\":\"{entry.Type}\"}}}}")
					.AppendLine(entry.Content);

				StoreBuffer(_waitingContent.ToString());
			}

			_worker.Start();
		}

		private async Task SendAnyPendingContentAsync()
		{
			while (true)
			{
				using (await _runMutex.LockAsync())
				{
					await _contentSemaphore.WaitAsync();

					string contentToSend;
					using (await _stringBuilderMutex.LockAsync())
					{
						contentToSend = _waitingContent.ToString();
					}

					if (await Send(contentToSend))
					{
						using (await _stringBuilderMutex.LockAsync())
						{
							_waitingContent.Remove(0, contentToSend.Length);
							StoreBuffer(_waitingContent.ToString());
							if (_waitingContent.Length != 0)
							{
								_contentSemaphore.Release();
							}
						}
						_backOffStrategy.Reset();
					}
					else
					{
						await _backOffStrategy.Wait();
						_contentSemaphore.Release();
					}
				}
			}
			// ReSharper disable once FunctionNeverReturns
		}

		private void StoreBuffer(string bufferContent)
		{
			_storage.Save(bufferContent);
		}

		private async Task<bool> Send(string content)
		{
			try
			{
				using (var stringContent = new StringContent(content, Encoding.UTF8, "text/plain"))
				using (HttpResponseMessage response = await _client.PostAsync(_url, stringContent))
				{
					bool deleteLogs = response.IsSuccessStatusCode
								   || response.StatusCode == System.Net.HttpStatusCode.BadRequest;
					//En cas de bad request la request sera toujours bad, donc on ne fera que boucler...
					//... il serait bon de plutôt éliminer les traces qui sont incorrects, mais avec l'implementation actuelle ce n'est pas possible...
					//... donc en attendant on clear les logs autrement on use toute la data du device et plus aucun log n'arrive sur kibana.
					return deleteLogs;
				}
			}
			catch (Exception ex)
			{
#if DEBUG
				//ignore 
				System.Diagnostics.Debug.Write(ex.ToString());
#endif
			}

			return false;
		}
	}
}