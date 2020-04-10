using Android.Content;
using Android.Content.PM;
using Xmf2.Core.Services;

namespace Xmf2.Core.Droid.Services
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

		public string GetFullVersion()
		{
			PackageInfo packageInfo = _applicationContext.PackageManager.GetPackageInfo(_applicationContext.PackageName, 0);
			return $"{packageInfo.VersionName}.{packageInfo.VersionCode}";
		}
	}
}