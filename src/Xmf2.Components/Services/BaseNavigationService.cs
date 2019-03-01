using System;
using System.Threading.Tasks;
using Xmf2.Components.Events;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Services
{
	public abstract class BaseNavigationService : BaseServiceContainer
	{
		protected BaseNavigationService(IServiceLocator services) : base(services)
		{
			
		}

		protected TViewModelType CreateViewModel<TViewModelType>(string scopeId, Func<IServiceLocator, TViewModelType> factory) where TViewModelType : IComponentViewModel
		{
			IServiceLocator locator = Services.Scope(scopeId);
			locator.RegisterSingleton<IEventBus, EventBus>();
			return factory(locator);
		}

		public abstract Task HandleRoute(string route);
	}
}