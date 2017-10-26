using Xmf2.iOS.Controls.ItemControls;
using Xmf2.Rx.ViewModels;

namespace Xmf2.Rx.iOS.Controls.ItemsControl
{
	public interface IUIComponent<TViewModel> : IUIModelComponent<TViewModel> where TViewModel : BaseViewModel
	{
		TViewModel ViewModel { get; set; }
	}
}