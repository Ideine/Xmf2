using System;
using Splat;

namespace Xmf2.Commons.Rx.Extensions
{
	public static class LocatorExtensions
	{
#if INTERVENTION
		public static TService GetService<TService>(this IDependencyResolver resolver)
#else
		public static TService GetService<TService>(this IReadonlyDependencyResolver resolver)
#endif
		{
			return resolver.GetService<TService>(null);
		}

#if INTERVENTION
		public static TService GetServiceOrDefault<TService>(this IDependencyResolver resolver)
#else
		public static TService GetServiceOrDefault<TService>(this IReadonlyDependencyResolver resolver)
#endif
		{
			try
			{
				return resolver.GetService<TService>(null);
			}
			catch
			{
				return default;
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
	}
}