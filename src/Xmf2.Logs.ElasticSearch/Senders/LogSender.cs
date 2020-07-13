﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xmf2.Common.Extensions;
using Xmf2.Common.Tools;
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
			
			_worker = new BackgroundWorker(RunAsync);
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

		private async Task RunAsync()
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
				HttpResponseMessage response = await _client.PostAsync(_url, new StringContent(content, Encoding.UTF8, "text/plain"));

				return response.IsSuccessStatusCode;
			}
			catch (Exception)
			{
				//ignore 
			}

			return false;
		}
	}
}