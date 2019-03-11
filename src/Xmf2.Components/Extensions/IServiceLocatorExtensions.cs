using System;

namespace Xmf2.Components.Interfaces
{
	public static class IServiceLocatorExtensions
	{
		public static Lazy<TService> LazyResolver<TService>(this IServiceLocator serviceLocator)
		{
			return new Lazy<TService>(serviceLocator.Resolve<TService>);
		}
	}
}
