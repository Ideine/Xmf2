using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Commons.Workers
{
	public class BackgroundQueueWorker<TWorkerData>
	{
		private readonly ConcurrentQueue<TWorkerData> _workerQueue = new ConcurrentQueue<TWorkerData>();
		private readonly SemaphoreSlim _mutex = new SemaphoreSlim(0);
		private readonly Func<TWorkerData, Task> _workerCallback;

		public BackgroundQueueWorker(Func<TWorkerData, Task> workerCallback)
		{
			_workerCallback = workerCallback;

			Task.Run(() => Run());
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

				if (!_workerQueue.TryDequeue(out var workerData)) //should not happen
				{
					_mutex.Release();
					continue;
				}

				await _workerCallback(workerData);
			}
		}
	}

	public class BackgroundQueueWorker<TWorkerData, TKey, TResult>
	{
		private class WorkItem
		{
			public TKey Key { get; set; }

			public TWorkerData WorkerData { get; set; }

			public Action<TResult> CompletionCallback { get; set; }
		}

		private readonly Dictionary<TKey, TResult> _previousResult = new Dictionary<TKey, TResult>();
		private readonly ConcurrentQueue<WorkItem> _workerQueue = new ConcurrentQueue<WorkItem>();
		private readonly SemaphoreSlim _mutex = new SemaphoreSlim(0);

		private readonly Func<TWorkerData, Task<TResult>> _workerCallback;
		private readonly Func<TWorkerData, TKey> _keyGetter;
		private readonly Func<TWorkerData, TResult, bool> _canCacheResult;

		public BackgroundQueueWorker(Func<TWorkerData, Task<TResult>> workerCallback, Func<TWorkerData, TKey> keyGetter, Func<TWorkerData, TResult, bool> canCacheResult)
		{
			_workerCallback = workerCallback;
			_keyGetter = keyGetter;
			_canCacheResult = canCacheResult;

			Task.Run(() => Run());
		}

		public void Add(TWorkerData worker, Action<TResult> completionCallback)
		{
			TKey key = _keyGetter(worker);

			if (_previousResult.TryGetValue(key, out var result))
			{
				completionCallback(result);
				return;
			}

			_workerQueue.Enqueue(new WorkItem
			{
				Key = key,
				CompletionCallback = completionCallback,
				WorkerData = worker
			});
			_mutex.Release();
		}

        public void InitializeWith(IEnumerable<Tuple<TKey, TResult>> existingData)
        {
            foreach(var items in existingData)
            {
                _previousResult.Add(items.Item1, items.Item2);
            }
        }

		private async void Run()
		{
			while (true)
			{
				await _mutex.WaitAsync();

				if (!_workerQueue.TryDequeue(out var wit)) //should not happen
				{
					_mutex.Release();
					continue;
				}

				if (!_previousResult.TryGetValue(wit.Key, out var result))
				{
					result = await _workerCallback(wit.WorkerData);
					if (_canCacheResult(wit.WorkerData, result))
					{
						_previousResult.Add(wit.Key, result);
					}
				}
				wit.CompletionCallback(result);
			}
		}
	}
}
