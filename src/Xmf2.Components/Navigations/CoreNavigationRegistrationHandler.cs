using System;
using System.Threading.Tasks;
using Xmf2.Components.Events;
using Xmf2.Components.Extensions;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Services;

namespace Xmf2.Components.Navigations
{
	public abstract class CoreNavigationRegistrationHandler : BaseServiceContainer
	{
		//private readonly Lazy<IViewModelFactory> _factory;
		//private IViewModelFactory Factory => _factory.Value;

		//private readonly string _defaultPath;

		private readonly Lazy<INavigationService> _navigationService;
		protected INavigationService NavigationService => _navigationService.Value;

		protected CoreNavigationRegistrationHandler(IServiceLocator services/*, string defaultPath*/) : base(services)
		{
			//_factory = LazyResolver<IViewModelFactory>();
			_navigationService = LazyResolver<INavigationService>();
			//_defaultPath = defaultPath;
		}

		protected IComponentViewModel CreateViewModel<TViewModelType>(string id, Func<IServiceLocator, IComponentViewModel> newViewModel)
		{
			IServiceLocator locator = Services.Scope(id);
			locator.RegisterSingleton<IEventBus, EventBus>();
			return newViewModel(locator);//Factory.Create(locator, new Location<TViewModelType>(_defaultPath, id));
		}

		public void Register()
		{
			Register(NavigationService);
		}

		protected virtual void Register(INavigationService navigationService)
		{

		}

		protected ScreenDefinition CreateScreen<TViewModel>(string name, Func<IServiceLocator, IComponentViewModel> newViewModel) where TViewModel : IComponentViewModel
			=> new ScreenDefinition(name, _ => CreateViewModel<TViewModel>(name, newViewModel).WaitInitialize());

		protected ScreenDefinition CreateScreenWithoutInitialization<TViewModel>(string name, Func<IServiceLocator, IComponentViewModel> newViewModel) where TViewModel : IComponentViewModel
			=> new ScreenDefinition(name, _ => Task.FromResult(CreateViewModel<TViewModel>(name, newViewModel)));

		protected ScreenDefinition CreateScreen<TViewModel>(string name, string parameterName, Func<IServiceLocator, IComponentViewModel> newViewModel) where TViewModel : IComponentViewModel
			=> new ScreenDefinition(name, _ => CreateViewModel<TViewModel>($"{{{parameterName}}}", newViewModel).WaitInitialize());

		protected ScreenDefinition CreateScreenWithoutInitialization<TViewModel>(string name, string parameterName, Func<IServiceLocator, IComponentViewModel> newViewModel) where TViewModel : IComponentViewModel
			=> new ScreenDefinition(name, _ => Task.FromResult(CreateViewModel<TViewModel>($"{{{parameterName}}}", newViewModel)));
	}
}