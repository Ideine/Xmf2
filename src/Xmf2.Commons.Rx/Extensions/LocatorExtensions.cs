using System;

namespace Splat
{
	public static class LocatorExtensions
	{
		public static TService GetService<TService>(this IDependencyResolver resolver)
		{
			return resolver.GetService<TService>(null);
		}

		public static TService GetServiceOrDefault<TService>(this IDependencyResolver resolver)
		{
			try
			{
				return resolver.GetService<TService>(null);
			}
			catch
			{
				return default(TService);
			}
		}

		public static void RegisterLazySingleton<TInterface>(this IMutableDependencyResolver resolver, Func<TInterface> creator)
		{
			resolver.RegisterLazySingleton(() => creator(), typeof(TInterface));
		}

		public static void RegisterLazySingleton<TInterface, TImplementation>(this IMutableDependencyResolver resolver) where TImplementation : class, TInterface, new()
		{
			resolver.RegisterLazySingleton(() => new TImplementation(), typeof(TInterface));
		}

		public static TService Resolve<TService>(this object _)
		{
			return Locator.Current.GetService<TService>();
		}

		public static TService ResolveOrDefault<TService>(this object _)
		{
			return Locator.Current.GetServiceOrDefault<TService>();
		}
	}
}
