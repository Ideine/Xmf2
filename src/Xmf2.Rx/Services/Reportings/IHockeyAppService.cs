namespace Xmf2.Rx.Services.Reportings
{
	public interface IHockeyAppService
	{
		void EnableCrashReporting();

		void DisableCrashReporting();
	}
}