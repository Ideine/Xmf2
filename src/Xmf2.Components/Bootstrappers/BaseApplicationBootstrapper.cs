using Xmf2.Components.Interfaces;
using Xmf2.Components.Services;

namespace Xmf2.Components.Bootstrappers
{
	public abstract class BaseApplicationBootstrapper : IApplicationBootstrapper
	{
		public static IServiceLocator StaticServices { get; private set; }

		public IServiceLocator Services { get; }

		protected BaseApplicationBootstrapper() : this(new ServiceLocator()) { }

		protected BaseApplicationBootstrapper(IServiceLocator serviceLocator)
		{
			StaticServices = Services = serviceLocator;
		}

		public void Initialize()
		{
			RegisterFirstChance();
			RegisterServices();
		}

		public abstract void ShowFirstView();

		protected virtual void RegisterFirstChance() { }

		protected virtual void RegisterServices() { }
	}
}