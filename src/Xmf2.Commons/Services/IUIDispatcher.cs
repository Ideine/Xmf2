using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Commons.Services
{
	public interface IUIDispatcher
	{
		void OnMainThread(Action action);
        Task<T> EnqueueOnMainThread<T>(Func<Task<T>> func, CancellationToken cancellationToken = default(CancellationToken));
    }
}
