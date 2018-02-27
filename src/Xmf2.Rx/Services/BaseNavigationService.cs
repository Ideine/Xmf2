using System;
using System.Threading.Tasks;
using Xmf2.Rx.ViewModels;

namespace Xmf2.Rx.Services
{
	public abstract class BaseNavigationService
	{
		protected abstract IBaseViewPresenterService BasePresenter { get; }

		protected virtual async Task<TViewModel> Initialize<TViewModel>(TViewModel viewModel)
			where TViewModel : BaseViewModel
		{
			viewModel.LifecycleManager.Initialize();
			await viewModel.LifecycleManager.WaitForInitialization().ConfigureAwait(false);
			return viewModel;
		}

		public virtual async Task Close<TViewModel>(TViewModel viewModel)
			where TViewModel : BaseViewModel
		{
			if (await BasePresenter.IsCurrentViewModel(viewModel))
			{
				BasePresenter.Close();
			}
			else
			{
				throw new Exception("Trying to close a ViewModel but it's not the top of the stack !");
			}
		}
	}
}
