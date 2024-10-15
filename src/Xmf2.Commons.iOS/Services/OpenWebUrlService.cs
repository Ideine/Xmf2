using Foundation;
using SafariServices;
using UIKit;
using Xmf2.Commons.Services;

namespace Xmf2.Commons.iOS.Services
{
	public class OpenWebUrlService: IOpenWebUrlService
	{
        public void Open(string url)
        {
            if (NSThread.IsMain)
			{
				InternalOpenWebSite(url);
			}
			else
			{
				UIApplication.SharedApplication.InvokeOnMainThread(() => InternalOpenWebSite(url));
			}
		}

		private void InternalOpenWebSite(string url)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
			{
				SFSafariViewController controller = new SFSafariViewController(new NSUrl(url));

				if (UIApplication.SharedApplication?.KeyWindow?.RootViewController != null)
				{
					UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(controller, true, null);
				}
			}
			else
			{
				UIApplication.SharedApplication.OpenUrl(new NSUrl(url), new NSDictionary(), _ => { });
			}
		}
	}
}