using System;
using Xmf2.Core.Helpers;

namespace Xmf2.Core.Subscriptions
{
	public class EventSubscriber : EventSubscriber<Unit>
	{
		public EventSubscriber(Action subscribe, Action unsubscribe, bool autoSubscribe = true)
			: base(Unit.Default, _ => subscribe(), _ => unsubscribe(), autoSubscribe) { }
	}

	public class EventSubscriber<T> : IDisposable
	{
		private Action<T> _subscribe;
		private Action<T> _unsubscribe;
		private T _obj;
		private bool _subscribed;
		private bool _disposed;

		public EventSubscriber(T obj, Action<T> subscribe, Action<T> unsubscribe, bool autoSubscribe = true)
		{
			_obj = obj;
			_subscribe = subscribe;
			_unsubscribe = unsubscribe;

			if (autoSubscribe)
			{
				Subscribe();
			}
		}

		public void Subscribe()
		{
			if (_subscribed)
			{
				return;
			}

			_subscribed = true;
			_subscribe(_obj);
		}

		public void Unsubscribe()
		{
			if (!_subscribed)
			{
				return;
			}

			try
			{
				_subscribed = false;
				_unsubscribe(_obj);
			}
			catch (ObjectDisposedException e)
			{
				System.Diagnostics.Debug.WriteLine(e);
			}
			catch (NullReferenceException nre)
			{
				System.Diagnostics.Debug.WriteLine(nre);
			}
		}

		~EventSubscriber()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				Unsubscribe();
				_obj = default(T);
				_subscribe = null;
				_unsubscribe = null;
			}

			_disposed = true;
		}
	}
}