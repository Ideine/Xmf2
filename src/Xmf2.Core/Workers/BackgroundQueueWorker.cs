using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Core.Workers
{
	public class BackgroundQueueWorker<TWorkerData>
	{
		private readonly ConcurrentQueue<TWorkerData> _workerQueue = new ConcurrentQueue<TWorkerData>();
		private readonly SemaphoreSlim _mutex = new SemaphoreSlim(0);
		private readonly Func<TWorkerData, Task> _workerCallback;

		public BackgroundQueueWorker(Func<TWorkerData, Task> workerCallback)
		{
			_workerCallback = workerCallback;

			Task.Factory.StartNew(() => Run());
		}

		public void Add(TWorkerData worker)
		{
			_workerQueue.Enqueue(worker);
			_mutex.Release();
		}

		private async void Run()
		{
			while (true)
			{
				await _mutex.WaitAsync();

				TWorkerData workerData;
				if (!_workerQueue.TryDequeue(out workerData)) //should not happen
				{
					_mutex.Release();
					continue;
				}

				await _workerCallback(workerData);
			}
		}
	}

	public class BackgroundQueueWorker<TParameters, TResult>
	{
		private class WorkItem
		{
			public TParameters Parameters { get; set; }
			public Action<TResult> CompletionCallback { get; set; }
		}

		private readonly ConcurrentQueue<WorkItem> _workQueue = new ConcurrentQueue<WorkItem>();
		private readonly SemaphoreSlim _mutex = new SemaphoreSlim(0);
		private readonly Func<TParameters, Task<TResult>> _fuctionBody;

		public BackgroundQueueWorker(Func<TParameters, Task<TResult>> functionBody)
		{
			_fuctionBody = functionBody;
			Task.Factory.StartNew(() => Run());
		}

		public void Add(TParameters parameters, Action<TResult> callback)
		{
			_workQueue.Enqueue(new WorkItem
			{
				CompletionCallback = callback,
				Parameters = parameters
			});
			_mutex.Release();
		}

		private async void Run()
		{
			while (true)
			{
				await _mutex.WaitAsync();
				WorkItem wit;
				if (!_workQueue.TryDequeue(out wit)) //should not happen
				{
					_mutex.Release();
					continue;
				}
				TResult result = await _fuctionBody(wit.Parameters);
				wit.CompletionCallback(result);
			}
		}
	}
}
