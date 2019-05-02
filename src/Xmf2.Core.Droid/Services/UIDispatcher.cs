using System;
using System.Threading;
using System.Threading.Tasks;
using Android.OS;
using Xmf2.Core.Services;

namespace Xmf2.Core.Droid.Services
{
	public class UIDispatcher : IUIDispatcher
	{
		private readonly Handler _handler;

		public UIDispatcher()
		{
			_handler = new Handler(Looper.MainLooper);
		}

		public void OnMainThread(Action action)
		{
			_handler.Post(action);
		}
		
		public bool IsOnMainThread() => Looper.MyLooper() == Looper.MainLooper;

		public async Task<T> EnqueueOnMainThread<T>(Func<Task<T>> func, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (IsOnMainThread())
			{
				cancellationToken.ThrowIfCancellationRequested();
				return await func();
			}
			else
			{
				var taskCompletionSource = new TaskCompletionSource<T>();
				_handler.Post(async () =>
				{
					try
					{
						cancellationToken.ThrowIfCancellationRequested();
						taskCompletionSource.SetResult(await func());
					}
					catch (Exception ex)
					{
						taskCompletionSource.SetException(ex);
					}
				});
				return await taskCompletionSource.Task;
			}
		}
	}
}
