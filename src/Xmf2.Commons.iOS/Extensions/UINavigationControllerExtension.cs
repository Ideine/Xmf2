using System.Linq;

namespace UIKit
{
	public static class UINavigationControllerExtension
	{
		public static void SetViewController(this UINavigationController uiNavigationController, UIViewController controller, bool animated)
		{
			uiNavigationController.SetViewControllers(new UIViewController[] { controller }, animated);
		}

		public static bool TryToFindViewControllerInStackOfType<TViewController>(this UINavigationController uiNavigationController, out TViewController viewController) where TViewController : UIViewController
		{
			viewController = uiNavigationController?.ViewControllers?.OfType<TViewController>().FirstOrDefault();
			return viewController != null;
		}
	}
}