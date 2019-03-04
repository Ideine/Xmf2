using Xmf2.Components.Services;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Navigations;

namespace Xmf2.Components.Droid.Navigations
{
	public abstract class NavigationRegistrationHandler<TCoreHandler> : BaseServiceContainer
		where TCoreHandler : CoreNavigationRegistrationHandler
	{
		public TCoreHandler CoreHandler { get; }

		protected NavigationRegistrationHandler(IServiceLocator services, TCoreHandler coreHandler) : base(services)
		{
			CoreHandler = coreHandler;
		}

		public void Register()
		{
			IRegistrationPresenterService registrationPresenterService = Services.Resolve<IRegistrationPresenterService>();

			CoreHandler.Register();
			Register(registrationPresenterService);
		}

		protected virtual void Register(IRegistrationPresenterService presenterService) { }
	}
}