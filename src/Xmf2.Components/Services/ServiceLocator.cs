using System;
using System.Collections.Generic;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Services
{
	public class ServiceLocator : IServiceLocator
	{
		private enum ResolverType
		{
			Singleton,
			LazySingleton,
			Factory
		}

		private readonly IServiceLocator _parent;
		private readonly Dictionary<Type, object> _implementations = new Dictionary<Type, object>();
		private readonly Dictionary<Type, object> _factory = new Dictionary<Type, object>();
		private readonly Dictionary<Type, ResolverType> _resolverTypes = new Dictionary<Type, ResolverType>();
		private readonly Dictionary<string, IServiceLocator> _scopes = new Dictionary<string, IServiceLocator>();

		public ServiceLocator() { }

		private ServiceLocator(IServiceLocator parent)
		{
			_parent = parent;
		}

		public void RegisterSingleton<TInterface>(TInterface implementation)
		{
			_resolverTypes[typeof(TInterface)] = ResolverType.Singleton;
			_implementations[typeof(TInterface)] = implementation;
		}

		public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface, new() => RegisterSingleton<TInterface>(new TImplementation());

		public void RegisterLazySingleton<TInterface>(Func<IServiceLocator, TInterface> creator)
		{
			_resolverTypes[typeof(TInterface)] = ResolverType.LazySingleton;
			_factory[typeof(TInterface)] = creator;
		}

		public void RegisterLazySingleton<TInterface, TImplementation>() where TImplementation : TInterface, new() => RegisterLazySingleton<TInterface>(_ => new TImplementation());

		public void RegisterFactory<TInterface>(Func<IServiceLocator, TInterface> creator)
		{
			_resolverTypes[typeof(TInterface)] = ResolverType.Factory;
			_factory[typeof(TInterface)] = creator;
		}

		public void RegisterFactory<TInterface, TImplementation>() where TImplementation : TInterface, new() => RegisterFactory<TInterface>(_ => new TImplementation());

		public IServiceLocator Scope(string name)
		{
			if (_scopes.TryGetValue(name, out IServiceLocator result))
			{
				return result;
			}

			result = new ServiceLocator(this);
			_scopes[name] = result;
			return result;
		}

		public TInterface Resolve<TInterface>()
		{
			if (TryResolve(out TInterface result))
			{
				return result;
			}

			throw new InvalidOperationException($"No implementation found for type {typeof(TInterface)}");
		}

		public bool TryResolve<TInterface>(out TInterface implementation)
		{
			if (_resolverTypes.TryGetValue(typeof(TInterface), out ResolverType resolverType))
			{
				if (resolverType == ResolverType.Singleton || resolverType == ResolverType.LazySingleton)
				{
					if (_implementations.TryGetValue(typeof(TInterface), out object result))
					{
						implementation = (TInterface) result;
						return true;
					}

					if (resolverType == ResolverType.LazySingleton)
					{
						implementation = ((Func<IServiceLocator, TInterface>) _factory[typeof(TInterface)])(this);
						_implementations[typeof(TInterface)] = implementation;
						return true;
					}
				}
				else if (resolverType == ResolverType.Factory)
				{
					implementation = ((Func<IServiceLocator, TInterface>) _factory[typeof(TInterface)])(this);
					return true;
				}
			}

			if (_parent == null)
			{
				implementation = default;
				return false;
			}

			return _parent.TryResolve(out implementation);
		}
	}
}