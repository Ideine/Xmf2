using System;
using System.Threading.Tasks;
using Xmf2.Components.Events;
using Xmf2.Components.Extensions;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Services;
using Xmf2.NavigationGraph.Core;

namespace Xmf2.Components.Navigations
{
	public abstract class BaseNavigationService<TViewModel> : BaseServiceContainer
		where TViewModel : IComponentViewModel
	{
		protected delegate TViewModel ViewModelFactory(IServiceLocator s);

		protected INavigationService<TViewModel> NavigationService { get; }

		protected BaseNavigationService(IServiceLocator services) : base(services)
		{
			NavigationService = services.Resolve<INavigationService<TViewModel>>();
		}

		protected TViewModel CreateViewModel(string scopeId, ViewModelFactory factory)
		{
			IServiceLocator locator = Services.Scope(scopeId);
			locator.RegisterSingleton<IEventBus, EventBus>();
			return factory(locator);
		}

		protected ScreenDefinition<TViewModel> CreateScreen(string name, ViewModelFactory factory)
			=> new ScreenDefinition<TViewModel>(name, _ => CreateViewModel(name, factory).WaitInitialize());

		protected ScreenDefinition<TViewModel> CreateScreenWithoutInitialization(string name, ViewModelFactory factory)
			=> new ScreenDefinition<TViewModel>(name, _ => Task.FromResult(CreateViewModel(name, factory)));

		protected ScreenDefinition<TViewModel> CreateScreen(string name, string parameterName, ViewModelFactory factory)
			=> new ScreenDefinition<TViewModel>(name, _ => CreateViewModel($"{{{parameterName}}}", factory).WaitInitialize());

		protected ScreenDefinition<TViewModel> CreateScreenWithoutInitialization(string name, string parameterName, ViewModelFactory factory)
			=> new ScreenDefinition<TViewModel>(name, _ => Task.FromResult(CreateViewModel($"{{{parameterName}}}", factory)));

		public void Register() => Register(NavigationService);
		protected virtual void Register(INavigationService<TViewModel> navigationService) { }

		public abstract Task HandleRoute(string route);
	}
}