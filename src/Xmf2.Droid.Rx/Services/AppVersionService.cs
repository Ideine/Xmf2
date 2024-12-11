using Android.Content;
using Android.Content.PM;
using AndroidX.Core.Content.PM;
using Xmf2.Commons.Services.Versions;

namespace Xmf2.Droid.Rx.Services
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
			using PackageInfo packageInfo = _applicationContext.PackageManager!.GetPackageInfo(_applicationContext.PackageName!, 0);
			return packageInfo!.VersionName;
		}

		public string GetFullVersion()
		{
			using PackageInfo packageInfo = _applicationContext.PackageManager!.GetPackageInfo(_applicationContext.PackageName!, 0);
			long versionCode = PackageInfoCompat.GetLongVersionCode(packageInfo);
			return $"{packageInfo!.VersionName}:{versionCode.ToString()}";
		}
	}
}