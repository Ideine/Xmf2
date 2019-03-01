using System;
using System.Threading.Tasks;

namespace Xmf2.Components.Interfaces
{
	public interface IViewModelOperation : IDisposable
	{
		IViewModelOperation ViewModelUpdate(Action update);

		IViewModelOperation<TResult> ViewModelUpdate<TResult>(Func<TResult> update);

		IViewModelOperation Async(Func<Task> asyncAction, bool withBusy = true, IBusy specificBusy = null);

		IViewModelOperation<TResult> Async<TResult>(Func<Task<TResult>> asyncAction, bool withBusy = true, IBusy specificBusy = null);

		Task Start();
	}

	public interface IViewModelOperation<out TParam> : IViewModelOperation
	{
		IViewModelOperation ViewModelUpdate(Action<TParam> update);

		IViewModelOperation<TResult> ViewModelUpdate<TResult>(Func<TParam, TResult> update);

		IViewModelOperation Async(Func<TParam, Task> asyncAction, bool withBusy = true, IBusy specificBusy = null);

		IViewModelOperation<TResult> Async<TResult>(Func<TParam, Task<TResult>> asyncAction, bool withBusy = true, IBusy specificBusy = null);
	}
}