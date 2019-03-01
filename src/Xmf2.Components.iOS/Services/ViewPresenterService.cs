using System;
using System.Threading.Tasks;
using UIKit;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.iOS.Views;
using Xmf2.Core.Helpers;

namespace Xmf2.Components.iOS.Services
{
	public class ViewPresenterService : IViewPresenterService
	{
		protected UINavigationController _navigationController;

		public ViewPresenterService(UIWindow window, bool handleRotation = false)
		{
			if (handleRotation)
			{
				_navigationController = new UINavigationController();
			}
			else
			{
				_navigationController = new HandleFreeRotateNavigationController();
			}
			window.RootViewController = _navigationController;
		}

		public bool IsCurrentViewFor<TViewModel>()
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

			InvokeOnMainThread(() =>
			{
				tcs.TrySetResult(_navigationController.TopViewController is IViewFor viewFor && viewFor.ViewModelType == typeof(TViewModel));
			});

			return tcs.Task.Result;
		}

		public virtual void Close() => Close(animated: true);
		public virtual void Close(bool animated)
		{
			InvokeOnMainThread(() =>
			{
				if (TryCloseModal(animated))
				{
					return;
				}
				else if (_navigationController.ViewControllers.Length > 0)
				{
					_navigationController.PopViewController(animated);
				}
				//Nothing left to close !
			});
		}

		private bool TryCloseModal(bool animated = true, Action completionHander = null)
		{
			var modalVC = GetModalViewController();
			if (modalVC != null)
			{
				modalVC?.DismissViewController(animated, completionHander);
				return true;
			}
			else
			{
				completionHander?.Invoke();
				return false;
			}
		}

		public virtual void ShowModalView<TViewController>(Func<TViewController> viewCreator) where TViewController : UIViewController
		{
			InvokeOnMainThread(() =>
			{
				var modalVc = _navigationController.PresentedViewController;
				if (modalVc == null)
				{
					_navigationController.PresentViewController(viewCreator(), true, ActionHelper.NoOp);
				}
				else
				{
					modalVc.DismissViewController(animated: true, completionHandler: () =>
					 {
						 _navigationController.PresentViewController(viewCreator(), true, ActionHelper.NoOp);
					 });
				}
			});
		}

		public void ShowRoot<TViewController>(bool animated = true, bool forceCreate = false, Action<TViewController> onFinished = null) where TViewController : UIViewController, new()
		{
			InvokeOnMainThread(() =>
			{
				TryCloseModal();
				if (!forceCreate && _navigationController.TopViewController is TViewController vc)
				{
					onFinished?.Invoke(vc);
					return;
				}
				else if (!forceCreate && _navigationController.ViewControllers.Length > 1 && _navigationController.ViewControllers[0] is TViewController)
				{
					_navigationController.PopToRootViewController(animated);
					if (_navigationController.TopViewController is TViewController viewController)
					{
						onFinished?.Invoke(viewController);
					}
				}
				else
				{
					vc = new TViewController();
					_navigationController.SetViewControllers(new UIViewController[] { vc }, animated);
					onFinished?.Invoke(vc);
				}
			});
		}

		public void Show<TBaseViewController, TViewController1>(bool animated = true)
			where TBaseViewController : UIViewController, new()
			where TViewController1 : UIViewController, new()
		{
			InvokeOnMainThread(() =>
			{
				UIViewController root = _navigationController.GetOrCreate<TBaseViewController>();
				UIViewController vc1 = new TViewController1();
				_navigationController.SetViewControllers(new[] { root, vc1 }, animated);
				TryCloseModal();
			});
		}

		public void Show<TBaseViewController, TViewController1, TViewController2>(bool animated = true, bool forceCreate = false)
			where TBaseViewController : UIViewController, new()
			where TViewController1 : UIViewController, new()
			where TViewController2 : UIViewController, new()
		{
			InvokeOnMainThread(() =>
			{
				UIViewController root = _navigationController.GetOrCreate<TBaseViewController>();
				UIViewController vc1 = _navigationController.GetOrCreate<TViewController1>();
				UIViewController vc2 = new TViewController2();
				_navigationController.SetViewControllers(new[] { root, vc1, vc2 }, animated);
				TryCloseModal();
			});
		}

		public void Show<TBaseViewController, TViewController1, TViewController2, TViewController3>(bool animated = true, bool forceCreateFirst = false, bool forceCreateSecond = false)
			where TBaseViewController : UIViewController, new()
			where TViewController1 : UIViewController, new()
			where TViewController2 : UIViewController, new()
			where TViewController3 : UIViewController, new()
		{
			InvokeOnMainThread(() =>
			{
				UIViewController root = _navigationController.GetOrCreate<TBaseViewController>();
				UIViewController vc1 = _navigationController.GetOrCreate<TViewController1>();
				UIViewController vc2 = _navigationController.GetOrCreate<TViewController2>();
				UIViewController vc3 = new TViewController3();
				_navigationController.SetViewControllers(new[] { root, vc1, vc2, vc3 }, animated);
				TryCloseModal();
			});
		}

		public void PopTo<TViewController>(bool animateModalClose = true, bool animatePop = true) where TViewController : UIViewController
		{
			InvokeOnMainThread(() =>
			{
				if (animateModalClose && animatePop)
				{
					TryCloseModal(animateModalClose, completionHander: DoPop);
				}
				else
				{
					TryCloseModal(animateModalClose);
					DoPop();
				}

				void DoPop()
				{
					if (_navigationController.TryToFindViewControllerInStackOfType(out TViewController viewController))
					{
						_navigationController.PopToViewController(viewController, animated: animatePop);
					}
				}
			});
		}

		public void Push<TViewController>(bool animateModalClose = true, bool animatePush = true) where TViewController : UIViewController, new()
		{
			this.Push(() => new TViewController(), animateModalClose, animatePush);
		}
		public void Push(Func<UIViewController> viewControllerFactory, bool animateModalClose = true, bool animatePush = true)
		{
			InvokeOnMainThread(() =>
			{
				if (animateModalClose && animatePush)
				{
					TryCloseModal(animateModalClose, completionHander: DoPush);
				}
				else
				{
					TryCloseModal(animateModalClose);
					DoPush();
				}

				void DoPush()
				{
					_navigationController.PushViewController(viewControllerFactory(), animatePush);
				}
			});
		}

		protected virtual UIViewController GetModalViewController()
		{
			return _navigationController.PresentedViewController;
		}

		protected static void InvokeOnMainThread(Action action)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(action);
		}
	}
}