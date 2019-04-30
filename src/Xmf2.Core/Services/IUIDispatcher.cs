using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Core.Services
{
	public interface IUIDispatcher
	{
		bool IsOnMainThread();
		void OnMainThread(Action action);
		Task<T> EnqueueOnMainThread<T>(Func<Task<T>> func, CancellationToken cancellationToken = default(CancellationToken));
	}
}
