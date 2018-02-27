using System;
using System.Threading.Tasks;
using Xmf2.Rx.ViewModels;

namespace Xmf2.Rx.Services
{
	public interface IBaseViewPresenterService
	{
		Task<bool> IsCurrentViewModel<TViewModel>(TViewModel viewModel) where TViewModel : BaseViewModel;
		void Close();
	}
}
