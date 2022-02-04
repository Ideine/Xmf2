using Xmf2.Commons.MvxExtends.ViewModels;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.ViewModels;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
	public interface IUIComponent : IMvxBindable
	{
		void AutoLayout();
		void Bind();
		void ViewDidLoad();
		void ViewDidAppear();
	}

	public interface IUIComponent<TViewModel> : IUIComponent where TViewModel : IMvxViewModel
	{
		TViewModel ViewModel { get; set; }
	}
}
