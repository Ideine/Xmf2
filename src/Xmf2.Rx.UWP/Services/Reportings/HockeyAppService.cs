using Microsoft.HockeyApp;
using Xmf2.Rx.Services.Reportings;

namespace Xmf2.Rx.UWP.Services.Reportings
{
    public class HockeyAppService : IHockeyAppService
    {
        private readonly string _appId;

        private IHockeyClient _manager;

        public HockeyAppService(string appId)
        {
            _manager = Microsoft.HockeyApp.HockeyClient.Current;
        }

        public void EnableCrashReporting()
        {
            _manager.Configure(_appId,new TelemetryConfiguration() { EnableDiagnostics = true});
        }

        public void DisableCrashReporting()
        {
            _manager.Configure(_appId, new TelemetryConfiguration() { EnableDiagnostics = false });
        }
    }
}
