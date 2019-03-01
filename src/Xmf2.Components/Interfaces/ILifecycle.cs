using System.Threading.Tasks;

namespace Xmf2.Components.Interfaces
{
	internal interface ILifecycle
	{
		/// <summary>
		/// Method is called prior to any navigation and should be used to 
		/// load data needed when view is displayed.
		/// </summary>
		/// <returns>Awaitable task</returns>
		Task Initialize();

		/// <summary>
		/// Method called on the first load of view.
		/// </summary>
		Task OnStart();

		/// <summary>
		/// Method called when view is displayed
		/// </summary>
		Task OnResume();

		/// <summary>
		/// Method called when view is hidden
		/// </summary>
		Task OnPause();

		/// <summary>
		/// Method called when view is stopped
		/// </summary>
		Task OnStop();
	}
}