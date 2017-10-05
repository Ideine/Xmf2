using HockeyApp.iOS;
using Xmf2.Rx.Services.Reportings;

namespace Xmf2.Rx.iOS.Services.Reportings
{
	public class HockeyAppService : IHockeyAppService
	{
		private readonly string _appId;

		public HockeyAppService(string appId)
		{
			_appId = appId;
			
			BITHockeyManager hockeyAppManager = BITHockeyManager.SharedHockeyManager;
			hockeyAppManager.Configure(_appId);
			hockeyAppManager.StartManager();
		}

		public void EnableCrashReporting()
		{
			BITHockeyManager hockeyAppManager = BITHockeyManager.SharedHockeyManager;
			hockeyAppManager.DisableCrashManager = false;
			hockeyAppManager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.AutoSend;
		}

		public void DisableCrashReporting()
		{
			BITHockeyManager hockeyAppManager = BITHockeyManager.SharedHockeyManager;
			hockeyAppManager.DisableCrashManager = true;
			hockeyAppManager.CrashManager.CrashManagerStatus = BITCrashManagerStatus.Disabled;
		}
	}
}