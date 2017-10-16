using System.Runtime.CompilerServices;

namespace UIKit
{
	public static class UINavigationControllerExtension
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetViewController(this UINavigationController uiNavigationController, UIViewController controller, bool animated)
		{
			uiNavigationController.SetViewControllers(new[] { controller }, animated);
		}
	}
}