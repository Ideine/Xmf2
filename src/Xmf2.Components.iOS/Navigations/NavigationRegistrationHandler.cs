using Xmf2.Components.Interfaces;
using Xmf2.Components.Navigations;
using Xmf2.Components.Services;
using Xmf2.NavigationGraph.iOS.Interfaces;

namespace Xmf2.Components.iOS.Navigations
{
	public class NavigationRegistrationHandler<TCoreHandler, TViewModel> : BaseServiceContainer
		where TCoreHandler : BaseNavigationService<TViewModel>
		where TViewModel : IComponentViewModel
	{
		public TCoreHandler CoreHandler { get; }

		protected NavigationRegistrationHandler(IServiceLocator services, TCoreHandler coreHandler) : base(services)
		{
			CoreHandler = coreHandler;
		}

		public void Register()
		{
			var registrationPresenterService = Services.Resolve<IRegistrationPresenterService<TViewModel>>();

			CoreHandler.Register();
			Register(registrationPresenterService);
		}

		protected virtual void Register(IRegistrationPresenterService<TViewModel> presenterService) { }
	}
}