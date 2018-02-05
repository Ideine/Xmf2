using Foundation;
using System;
using Xmf2.Commons.Services.Versions;

namespace Xmf2.Commons.iOS.Services
{
	public class AppVersionService : IAppVersionService
	{
        public string GetVersion()
		{
			//CFBundleVersion : 1.0.0
			//CFBundleShortVersionString : 1.0
			return NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
		}

        public string GetBuildVersion()
        {
            var appVersion = Version.Parse(GetVersion());
            return appVersion.Build.ToString();
        }
    }
}
