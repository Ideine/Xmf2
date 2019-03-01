using UIKit;
namespace Xmf2.Components.iOS.Views
{
	public class HandleFreeRotateNavigationController : UINavigationController
	{
		public HandleFreeRotateNavigationController() { }

		public HandleFreeRotateNavigationController(UIViewController rootViewController) : base(rootViewController) { }

		public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
		{
			return VisibleViewController?.PreferredInterfaceOrientationForPresentation() ?? base.PreferredInterfaceOrientationForPresentation();
		}

		public override bool ShouldAutorotate()
		{
			return VisibleViewController?.ShouldAutorotate() ?? base.ShouldAutorotate();
		}

		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return VisibleViewController?.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation) ?? base.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		{
			return VisibleViewController?.GetSupportedInterfaceOrientations() ?? base.GetSupportedInterfaceOrientations();
		}
	}
}
