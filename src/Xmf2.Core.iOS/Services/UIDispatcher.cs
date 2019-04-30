using System;
using System.Threading;
using System.Threading.Tasks;
using CoreFoundation;
using Foundation;
using UIKit;
using Xmf2.Core.Services;

namespace Xmf2.Core.iOS.Services
{
	public class UIDispatcher : IUIDispatcher
	//{
	//	public void OnMainThread(Action action)
	//	{
	//		UIApplication.SharedApplication.InvokeOnMainThread(action);
	//	}
	//}
	{
		public bool IsOnMainThread() => NSThread.IsMain;

		public void OnMainThread(Action action) => UIApplication.SharedApplication.InvokeOnMainThread(action);

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
				DispatchQueue.MainQueue.DispatchAsync(async () =>
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
