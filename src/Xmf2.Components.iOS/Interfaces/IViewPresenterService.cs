using System;
using UIKit;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.iOS.Interfaces
{
	public interface IViewPresenterService : IPresenterService
	{
		bool IsCurrentViewFor<TViewModel>();

		void ShowModalView<TViewController>(Func<TViewController> viewCreator) where TViewController : UIViewController;

		void ShowRoot<TViewController>(bool animated = true, bool forceCreate = false, Action<TViewController> onFinished = null) where TViewController : UIViewController, new();

		void Show<TBaseViewController, TViewController>(bool animated = true)
			where TBaseViewController : UIViewController, new()
			where TViewController : UIViewController, new();

		void Show<TBaseViewController, TViewController, TViewControllerBis>(bool animated = true, bool forceCreate = false)
			where TBaseViewController : UIViewController, new()
			where TViewController : UIViewController, new()
			where TViewControllerBis : UIViewController, new();

		void Show<TBaseViewController, TViewController, TViewControllerBis, TViewControllerTer>(bool animated = true, bool forceCreateFirst = false, bool forceCreateSecond = false)
			where TBaseViewController : UIViewController, new()
			where TViewController : UIViewController, new()
			where TViewControllerBis : UIViewController, new()
			where TViewControllerTer : UIViewController, new();

		void Push<TViewController>(bool animateModalClose = true, bool animatePop = true)
			where TViewController : UIViewController, new();
	}
}