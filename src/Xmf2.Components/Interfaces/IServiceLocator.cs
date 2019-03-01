using System;

namespace Xmf2.Components.Interfaces
{
	public interface IServiceLocator
	{
		void RegisterSingleton<TInterface>(TInterface implementation);

		void RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface, new();

		void RegisterLazySingleton<TInterface>(Func<IServiceLocator, TInterface> creator);

		void RegisterLazySingleton<TInterface, TImplementation>() where TImplementation : TInterface, new();

		void RegisterFactory<TInterface>(Func<IServiceLocator, TInterface> creator);

		void RegisterFactory<TInterface, TImplementation>() where TImplementation : TInterface, new();

		IServiceLocator Scope(string name);

		TInterface Resolve<TInterface>();

		bool TryResolve<TInterface>(out TInterface implementation);
	}
}