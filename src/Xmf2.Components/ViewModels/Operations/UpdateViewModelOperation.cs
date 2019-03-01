using System;
using System.Threading.Tasks;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels.Operations
{
	internal class UpdateViewModelOperation<TParam, TResult> : ViewModelOperation<TResult>
	{
		private Func<Task<TParam>> _previousRun;
		private Func<TParam, TResult> _run;

		public UpdateViewModelOperation(Func<Task<TParam>> previousRun, Func<TParam, TResult> run, IBusy busy) : base(busy)
		{
			_previousRun = previousRun;
			_run = run;
		}

		protected override async Task<TResult> Execute()
		{
			TParam previousResult = await _previousRun();

			lock (ApplicationState.Mutex)
			{
				TResult result = _run(previousResult);
				return result;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				_previousRun = null;
				_run = null;
			}
		}
	}
}