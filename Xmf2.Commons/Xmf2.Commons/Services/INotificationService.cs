using System.Threading.Tasks;

namespace Xmf2.Commons.Services
{
	public interface INotificationService
	{
		void SetToken(string token);

		Task RegisterForNotification();

		Task UnregisterForNotification();
	}
}