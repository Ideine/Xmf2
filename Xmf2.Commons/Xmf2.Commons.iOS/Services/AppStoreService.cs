using Foundation;
using UIKit;
using Xmf2.Commons.Services.Versions;

namespace Xmf2.Commons.iOS.Services
{
	public class AppStoreService : IAppStoreService
	{
		private const string IOS_APPSTORE_URL_FORMAT = "https://itunes.apple.com/in/app/id{0}?mt=8";
		private readonly string _updateUrl;

		public AppStoreService(string appleId)
		{
			_updateUrl = string.Format(IOS_APPSTORE_URL_FORMAT, appleId);
		}

		public void OpenUpdatePage()
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(_updateUrl));
		}
	}
}