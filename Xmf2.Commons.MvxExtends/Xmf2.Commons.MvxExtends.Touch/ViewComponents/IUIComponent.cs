using MvvmCross.Platforms.Ios.Binding.Views;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
    public interface IUIComponent : IMvxBindable
	{
		void AutoLayout();
		void Bind();
		void ViewDidLoad();
		void ViewDidAppear();
	}

	public interface IUIComponent<TViewModel> : IUIComponent where TViewModel : BaseViewModel<object>
	{
		TViewModel ViewModel { get; set; }
	}
}
