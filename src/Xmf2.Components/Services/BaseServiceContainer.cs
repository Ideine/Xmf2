using System;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Services
{
	public abstract class BaseServiceContainer
	{
		protected IServiceLocator Services { get; }

		protected BaseServiceContainer(IServiceLocator services)
		{
			Services = services;
		}

		public Lazy<TService> LazyResolver<TService>()
		{
			return new Lazy<TService>(Services.Resolve<TService>);
		}
	}
}