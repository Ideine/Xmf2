namespace UIKit
{
	public static class UINavigationControllerExtension
	{
		public static void SetViewController(this UINavigationController uiNavigationController, UIViewController controller, bool animated)
		{
			uiNavigationController.SetViewControllers(new UIViewController[] { controller }, animated);
		}
	}
}