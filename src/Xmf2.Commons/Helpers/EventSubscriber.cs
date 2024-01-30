using System;
using System.Diagnostics;

namespace Xmf2.Commons.Helpers
{
	public class EventSubscriber : EventSubscriber<Xmf2Unit>
	{
		public EventSubscriber(Action subscribe, Action unsubscribe, bool autoSubscribe = true) : base(Xmf2Unit.Default, _ => subscribe(), _ => unsubscribe(), autoSubscribe) { }
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
				Debug.WriteLine(e);
			}
			catch (NullReferenceException nre)
			{
				Debug.WriteLine(nre);
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