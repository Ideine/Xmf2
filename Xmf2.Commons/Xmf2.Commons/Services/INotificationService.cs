namespace Xmf2.Commons.Services
{
	public interface INotificationService
	{
		void SetToken(string token);

		void RegisterForNotification();

		void UnregisterForNotification();
	}
}