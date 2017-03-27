using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Rest.Caches
{
	public static class CacheEngine
	{
		public const string SCOPE_SESSION = nameof(SCOPE_SESSION);
		public const string SCOPE_USER = nameof(SCOPE_USER);
		public const string SCOPE_APP = nameof(SCOPE_APP);
		
		private class CacheItem<T, TParam> : ICacheItem<T>, ICacheItem<T, TParam> where T : class
		{
			private readonly TimeSpan _validityTime;
			private readonly Func<TParam, CancellationToken, Task<T>> _loader;
			private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);
			private T _value;
			private DateTime _expireDate;

			public T Value => _value;

			internal CacheItem(TimeSpan validityTime, Func<TParam, CancellationToken, Task<T>> loader)
			{
				_loader = loader;
				_validityTime = validityTime;
			}

			public void Invalidate()
			{
				_value = null;
				_expireDate = DateTime.MinValue;
			}

			public Task<T> Load(bool force = false) => Load(default(TParam), CancellationToken.None, force);

			public Task<T> Load(CancellationToken ct, bool force = false) => Load(default(TParam), ct, force);

			public Task<T> Load(TParam param, bool force = false) => Load(param, CancellationToken.None, force);

			public async Task<T> Load(TParam param, CancellationToken ct, bool force = false)
			{
				if (_value == null || DateTime.Now > _expireDate || force)
				{
					await _mutex.WaitAsync(ct);
					ct.ThrowIfCancellationRequested();
					try
					{
						if (_value == null || DateTime.Now > _expireDate || force)
						{
							_expireDate = DateTime.Now.Add(_validityTime);
							_value = await _loader(param, ct);
						}
					}
					finally
					{
						_mutex.Release();
					}
				}
				return _value;
			}
		}

		private static readonly Dictionary<string, List<ICacheItem>> _itemsPerScope = new Dictionary<string, List<ICacheItem>>();
		
		public static void InvalidateScope(string scope)
		{
			if (_itemsPerScope.ContainsKey(scope))
			{
				foreach (ICacheItem item in _itemsPerScope[scope])
				{
					item.Invalidate();
				}
			}
		}

		public static ICacheItem<T> CreateCacheItem<T>(string scope, TimeSpan validityTime, Func<CancellationToken, Task<T>> loader) where T : class
		{
			CacheItem<T, object> item = new CacheItem<T, object>(validityTime, (p, ct) => loader(ct));

			if (!_itemsPerScope.ContainsKey(scope))
			{
				_itemsPerScope.Add(scope, new List<ICacheItem>());
			}
			_itemsPerScope[scope].Add(item);

			return item;
		}

		public static ICacheItem<T, TParam> CreateCacheItem<T, TParam>(string scope, TimeSpan validityTime, Func<TParam, CancellationToken, Task<T>> loader) where T : class
		{
			CacheItem<T, TParam> item = new CacheItem<T, TParam>(validityTime, loader);

			if (!_itemsPerScope.ContainsKey(scope))
			{
				_itemsPerScope.Add(scope, new List<ICacheItem>());
			}
			_itemsPerScope[scope].Add(item);

			return item;
		}
	}
}