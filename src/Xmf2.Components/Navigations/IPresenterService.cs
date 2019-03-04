using System.Threading.Tasks;

namespace Xmf2.Components.Navigations
{
	public interface IPresenterService
	{
		Task UpdateNavigation(NavigationOperation navigationOperation, INavigationInProgress navigationInProgress);

		void CloseApp();
	}
}