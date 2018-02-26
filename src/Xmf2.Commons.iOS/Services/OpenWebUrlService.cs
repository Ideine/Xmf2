﻿using System;
using Foundation;
using SafariServices;
using UIKit;
using Xmf2.Commons.Services;

namespace Xmf2.Commons.iOS.Services
{
	public class OpenWebUrlService: IOpenWebUrlService
	{
		public void OpenWebsite(string url)
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
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				UIApplication.SharedApplication.OpenUrl(new NSUrl(url), new NSDictionary(), _ => { });
			}
			else
			{
				SFSafariViewController controller = new SFSafariViewController(new NSUrl(url));

				if (UIApplication.SharedApplication?.KeyWindow?.RootViewController != null)
				{
					UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(controller, true, null);
				}
			}
		}
	}
}