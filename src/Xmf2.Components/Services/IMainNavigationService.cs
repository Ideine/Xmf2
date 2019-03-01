using System.Threading.Tasks;

namespace Xmf2.Components.Services
{
	public interface IMainNavigationService
	{
		Task HandleDeeplink(string route);
		void RegisterModuleNavigationService(string route, BaseNavigationService moduleNavigationService);
		Task BackToRoot();
	}
}