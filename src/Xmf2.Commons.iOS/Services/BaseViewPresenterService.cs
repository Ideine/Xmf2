using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Xmf2.Commons.iOS.Services
{
	public abstract class BaseViewPresenterService
	{
		protected UIWindow Window { get; }

		protected UINavigationController NavigationController { get; set; }

		protected UIViewController _topViewController => NavigationController?.TopViewController;

		public BaseViewPresenterService(UIWindow window)
		{
			Window = window;
		}
		public void Close()
		{
			if (NavigationController == null)
			{
				return;
			}
			InvokeOnMainThread(() =>
			{
				if (NavigationController.ViewControllers.Length > 0)
				{
					var modalVc = GetModalViewController();
					if (modalVc != null)
					{
						modalVc.DismissViewController(true, null);
					}
					else
					{
						NavigationController.PopViewController(animated: true);
					}
				}
			});
		}
		protected virtual void ShowModalView<TViewController>(Func<TViewController> viewCreator) where TViewController : UIViewController
		{
			InvokeOnMainThread(() =>
			{
				var modalVC = UIApplication.SharedApplication.KeyWindow.RootViewController.PresentedViewController;
				if (modalVC == null)
				{
					NavigationController.PresentViewController(viewCreator(), animated: true, completionHandler: ActionHelper.NoOp);
				}
				else
				{
					modalVC.PresentViewController(viewCreator(), animated: true, completionHandler: ActionHelper.NoOp);
				}
			});
		}
		protected virtual void Show<TViewController>() where TViewController : UIViewController, new()
		{
			Show(() => new TViewController());
		}
		protected abstract void Show<TViewController>(Func<TViewController> viewCreator) where TViewController : UIViewController;
		protected abstract UIViewController GetModalViewController();
		protected void ReplaceView(Func<UIViewController> viewCreator)
		{
			InvokeOnMainThread(() =>
			{
				if (NavigationController == null)
				{
					NavigationController = new UINavigationController();
					Window.RootViewController = NavigationController;
				}
				var viewControllerArray = NavigationController.ViewControllers;
				viewControllerArray[viewControllerArray.Length - 1] = viewCreator();
				NavigationController.SetViewControllers(viewControllerArray, true);
			});
		}
		protected static void InvokeOnMainThread(Action action)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(action);
		}
		protected virtual void EnsureNavigationControllerIsSet()
		{
			if (NavigationController == null)
			{
				NavigationController = new UINavigationController();
				Window.RootViewController = NavigationController;
			}
		}
		protected bool IsInNavigationStack<TViewController>() where TViewController : UIViewController
		{
			var result = this.NavigationController?.ViewControllers?.Any(vc => vc is TViewController);
			if (result.HasValue)
			{
				return result.Value;
			}
			else
			{
				return false;
			}
		}
		protected bool TryToFindViewControllerInStackOfType<TViewController>(out UIViewController viewController) where TViewController : UIViewController
		{
			viewController = this.NavigationController?.ViewControllers?.FirstOrDefault(vc => vc is TViewController);
			return viewController != null;
		}
	}
}