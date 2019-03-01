using System;
using System.Threading.Tasks;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels.Operations
{
	internal class AsyncViewModelOperation<TParam, TResult> : ViewModelOperation<TResult>
	{
		private Func<Task<TParam>> _previousRun;
		private Func<TParam, Task<TResult>> _run;
		private IBusy _specificBusy;
		private readonly bool _withBusy;

		public AsyncViewModelOperation(Func<Task<TParam>> previousRun, Func<TParam, Task<TResult>> run, IBusy busy, IBusy specificBusy, bool withBusy) : base(busy)
		{
			_previousRun = previousRun;
			_run = run;
			_specificBusy = specificBusy;
			_withBusy = withBusy;
		}

		protected override async Task<TResult> Execute()
		{
			TParam previousResult = await _previousRun();

			if (_withBusy)
			{
				IBusy busy = _specificBusy ?? Busy;
				if (busy != null)
				{
					using (busy.EnableDisposable())
					{
						return await _run(previousResult);
					}
				}
			}

			return await _run(previousResult);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				_specificBusy = null;
				_previousRun = null;
				_run = null;
			}
		}
	}
}