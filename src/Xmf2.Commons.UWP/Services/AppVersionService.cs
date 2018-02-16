using System;
using Windows.ApplicationModel;
using Xmf2.Commons.Services.Versions;

namespace Xmf2.Commons.UWP.Services
{
    public class AppVersionService : IAppVersionService
    {              
        public string GetVersion()
        {
            PackageVersion pv = Package.Current.Id.Version;
            var appVersion = string.Format("{0}.{1}.{2}.{3}", pv.Major, pv.Minor, pv.Build, pv.Revision);
            return $"{appVersion}";
        }

        public string GetBuildVersion()
        {
            return Package.Current.Id.Version.Build.ToString();
        }

        public Version GetFullVersion()
        {
            return Version.Parse(GetVersion());
        }
    }
}
