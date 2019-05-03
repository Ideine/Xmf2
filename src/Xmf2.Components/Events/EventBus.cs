using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xmf2.Components.Events
{
	/// <remarks>
	/// L'implémentation de l'interface <see cref="IGlobalEventBus"/> en plus de <see cref="IEventBus"/>
	/// permet de différencier dans le <see cref="Xmf2.Components.Interfaces.IServiceLocator"/> a quelle instance il est fait référence.
	/// </remarks>
	public class EventBus : IEventBus, IGlobalEventBus
	{
		private readonly object _mutex = new object();
		private readonly LinkedList<IEventSubscription> _subscriptions = new LinkedList<IEventSubscription>();
		private readonly Dictionary<SubscriptionToken, LinkedListNode<IEventSubscription>> _nodes = new Dictionary<SubscriptionToken, LinkedListNode<IEventSubscription>>();

		public IDisposable Subscribe<TEvent>(Action callback) where TEvent : IEvent => Subscribe((TEvent _) => callback());

		public IDisposable Subscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent
		{
			SubscriptionToken token = new SubscriptionToken(this);
			IEventSubscription subscriptions = new StrongEventSubscription<TEvent>(token, callback);

			lock (_mutex)
			{
				_nodes.Add(token, _subscriptions.AddLast(subscriptions));
			}

			return token;
		}

		public virtual void Publish<TEvent>(TEvent evt) where TEvent : IEvent
		{
			if (evt == null)
			{
				return;
			}

			Task.Run(() =>
			{
				lock (_mutex)
				{
					foreach (IEventSubscription subscription in _subscriptions)
					{
						subscription.TryDeliver(evt);
					}
				}
			});
		}

		private void Unsubscribe(SubscriptionToken subscriptionToken)
		{
			lock (_mutex)
			{
				if (_nodes.TryGetValue(subscriptionToken, out LinkedListNode<IEventSubscription> node))
				{
					_subscriptions.Remove(node);
					node.Value.Dispose();
				}
			}
		}

		private class SubscriptionToken : IDisposable
		{
			private WeakReference<EventBus> _bus;

			public SubscriptionToken(EventBus bus)
			{
				_bus = new WeakReference<EventBus>(bus);
			}

			#region Dispose

			protected void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (_bus is null)
					{
						System.Diagnostics.Debug.WriteLine($"Warning: {nameof(EventBus)}.{nameof(Dispose)} called twice");
					}
					else
					{
						if (_bus.TryGetTarget(out EventBus bus))
						{
							bus.Unsubscribe(this);
						}
						_bus = null;
					}
				}
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			~SubscriptionToken()
			{
				Dispose(false);
			}

			#endregion
		}

		private interface IEventSubscription : IDisposable
		{
			bool HasToken(SubscriptionToken token);

			void TryDeliver(IEvent evt);
		}

		private class WeakEventSubscription<TEvent> : IEventSubscription
		{
			private SubscriptionToken _token;
			private WeakReference<Action<TEvent>> _callback;

			public WeakEventSubscription(SubscriptionToken token, Action<TEvent> callback)
			{
				_token = token;
				_callback = new WeakReference<Action<TEvent>>(callback);
			}

			public bool HasToken(SubscriptionToken token) => token == _token;

			public void TryDeliver(IEvent evt)
			{
				if (evt is TEvent typedEvent && _callback.TryGetTarget(out Action<TEvent> callback))
				{
					callback(typedEvent);
				}
			}

			#region Dispose

			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					_callback = null;
					_token = null;
				}
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			~WeakEventSubscription()
			{
				Dispose(false);
			}

			#endregion
		}

		private class StrongEventSubscription<TEvent> : IEventSubscription
		{
			private SubscriptionToken _token;
			private Action<TEvent> _callback;

			public StrongEventSubscription(SubscriptionToken token, Action<TEvent> callback)
			{
				_token = token;
				_callback = callback;
			}

			public bool HasToken(SubscriptionToken token) => token == _token;

			public void TryDeliver(IEvent evt)
			{
				if (evt is TEvent typedEvent)
				{
					_callback(typedEvent);
				}
			}

			#region Dispose

			protected virtual void Dispose(bool disposing)
			{
				if (disposing)
				{
					_callback = null;
					_token = null;
				}
			}

			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			~StrongEventSubscription()
			{
				Dispose(false);
			}

			#endregion
		}
	}
}