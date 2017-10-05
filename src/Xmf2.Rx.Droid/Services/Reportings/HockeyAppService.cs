using Android.Content;
using HockeyApp.Android;
using Xmf2.Rx.Services.Reportings;

namespace Xmf2.Rx.Droid.Services.Reportings
{
	public class HockeyAppService : IHockeyAppService
	{
		private readonly Context _context;
		private readonly string _appId;
		
		private readonly CustomCrashListener _crashListener = new CustomCrashListener();
		
		public HockeyAppService(Context context, string appId)
		{
			_context = context;
			_appId = appId;
		}

		public void EnableCrashReporting()
		{
			_crashListener.EnableCrashReport = true;
			CrashManager.Register(_context, _appId, _crashListener);
		}

		public void DisableCrashReporting()
		{
			_crashListener.EnableCrashReport = false;
		}

		private class CustomCrashListener : CrashManagerListener
		{
			public bool EnableCrashReport { get; set; }
			
			public override bool ShouldAutoUploadCrashes() => EnableCrashReport;

			public override bool OnHandleAlertView() => true; //disable showing hockeyapp alert view
		}
	}
}