using System;
using System.Threading.Tasks;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Helpers;

namespace Xmf2.Components.ViewModels.Operations
{
	internal abstract class ViewModelOperation<TParam> : IViewModelOperation<TParam>
	{
		protected IBusy Busy;

		protected ViewModelOperation(IBusy busy)
		{
			Busy = busy;
		}

		public IViewModelOperation ViewModelUpdate(Action update)
		{
			return new UpdateViewModelOperation<TParam, Unit>(Execute, _ =>
			{
				update();
				return Unit.Default;
			}, Busy);
		}

		public IViewModelOperation<TResult> ViewModelUpdate<TResult>(Func<TResult> update)
		{
			return new UpdateViewModelOperation<TParam, TResult>(Execute, _ => update(), Busy);
		}

		public IViewModelOperation ViewModelUpdate(Action<TParam> update)
		{
			return new UpdateViewModelOperation<TParam, Unit>(Execute, p =>
			{
				update(p);
				return Unit.Default;
			}, Busy);
		}

		public IViewModelOperation<TResult> ViewModelUpdate<TResult>(Func<TParam, TResult> update)
		{
			return new UpdateViewModelOperation<TParam, TResult>(Execute, update, Busy);
		}

		public IViewModelOperation Async(Func<TParam, Task> asyncAction, bool withBusy = true, IBusy specificBusy = null)
		{
			return new AsyncViewModelOperation<TParam, Unit>(Execute, async p =>
			{
				await asyncAction(p);
				return Unit.Default;
			}, Busy, specificBusy, withBusy);
		}

		public IViewModelOperation<TResult> Async<TResult>(Func<TParam, Task<TResult>> asyncAction, bool withBusy = true, IBusy specificBusy = null)
		{
			return new AsyncViewModelOperation<TParam, TResult>(Execute, asyncAction, Busy, specificBusy, withBusy);
		}

		public IViewModelOperation Async(Func<Task> asyncAction, bool withBusy = true, IBusy specificBusy = null)
		{
			return new AsyncViewModelOperation<TParam, Unit>(Execute, async _ =>
			{
				await asyncAction();
				return Unit.Default;
			}, Busy, specificBusy, withBusy);
		}

		public IViewModelOperation<TResult> Async<TResult>(Func<Task<TResult>> asyncAction, bool withBusy = true, IBusy specificBusy = null)
		{
			return new AsyncViewModelOperation<TParam, TResult>(Execute, _ => asyncAction(), Busy, specificBusy, withBusy);
		}

		public Task Start()
		{
			return Execute();
		}

		protected abstract Task<TParam> Execute();

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Busy = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ViewModelOperation()
		{
			Dispose(false);
		}

		#endregion
	}
}