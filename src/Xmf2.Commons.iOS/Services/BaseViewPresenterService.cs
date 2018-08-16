using System;
using System.Linq;
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
		public virtual void Close()
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
				var modalVC = TopMostViewController();
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
		protected virtual TViewController Show<TViewController>() where TViewController : UIViewController, new()
		{
			return Show<TViewController>(animated: true);
		}
		protected virtual TViewController Show<TViewController>(bool animated) where TViewController : UIViewController, new()
		{
			return Show(() => new TViewController(), animated);
		}
		protected virtual TViewController Show<TViewController>(Func<TViewController> viewCreator) where TViewController : UIViewController
		{
			return Show(viewCreator, animated: true);
		}
		protected abstract TViewController Show<TViewController>(Func<TViewController> viewCreator, bool animated) where TViewController : UIViewController;
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

		protected bool TryToFindViewControllerInStackOfType<TViewController>(out TViewController viewController) where TViewController : class
		{
			return this.NavigationController.TryToFindViewControllerInStackOfType(out viewController);
		}

		private UIViewController TopMostViewController()
		{
			UIViewController topController = UIApplication.SharedApplication.KeyWindow?.RootViewController.PresentedViewController;

			while (topController?.PresentedViewController != null)
			{
				topController = topController.PresentedViewController;
			}
			return topController;
		}
	}
}