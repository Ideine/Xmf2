using Foundation;
using Xmf2.Core.Services;

namespace Xmf2.Core.iOS.Services
{
	public class AppVersionService : IAppVersionService
	{
		public string GetVersion()
		{
			//CFBundleVersion : 1.0.0
			//CFBundleShortVersionString : 1.0
			return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
		}

		public string GetFullVersion()
		{
			//CFBundleVersion : 1.0.0
			//CFBundleShortVersionString : 1.0
			return NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
		}
	}
}