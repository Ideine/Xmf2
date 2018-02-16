using System;
using Android.Content;
using Android.Content.PM;
using Xmf2.Commons.Services.Versions;

namespace Xmf2.Commons.Droid.Services
{
    public class AppVersionService : IAppVersionService
    {
        private readonly Context _applicationContext;

        public AppVersionService(Context applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public string GetVersion()
        {
            return _applicationContext.PackageManager.GetPackageInfo(_applicationContext.PackageName, 0).VersionName;
        }

        public string GetBuildVersion()
        {
            PackageInfo packageInfo = _applicationContext.PackageManager.GetPackageInfo(_applicationContext.PackageName, 0);
            return packageInfo.VersionCode.ToString();
        }

		public Version GetFullVersion()
		{
			var packageInfo = _applicationContext.PackageManager.GetPackageInfo(_applicationContext.PackageName, 0);
			string fullVersion = $"{packageInfo.VersionName}.{packageInfo.VersionCode}";
			if(Version.TryParse(fullVersion, out Version result))
			{
				return result;
			}
			return new Version(0, 0, 0, 0);
		}
    }
}
