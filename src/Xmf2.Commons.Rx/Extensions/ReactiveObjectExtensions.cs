using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using ReactiveUI;
using Splat;

namespace Xmf2.Commons.Rx.Extensions
{
	public static class IReactiveObjectExtensions
	{
		private static readonly ConditionalWeakTable<IReactiveObject, IExtensionState<IReactiveObject>> _state = new();

		private interface IExtensionState<out TSender>
			where TSender : IReactiveObject
		{
			/// <summary>
			/// Gets an observable for when a property is changing.
			/// </summary>
			IObservable<IReactivePropertyChangedEventArgs<TSender>> Changing { get; }

			/// <summary>
			/// Gets an observable for when the property has changed.
			/// </summary>
			IObservable<IReactivePropertyChangedEventArgs<TSender>> Changed { get; }

			/// <summary>
			/// Gets a observable for when an exception is thrown.
			/// </summary>
			IObservable<Exception> ThrownExceptions { get; }

			/// <summary>
			/// Subscribe raise property changing events to a property changing
			/// observable. Must be called before raising property changing events.
			/// </summary>
			void SubscribePropertyChangingEvents();

			/// <summary>
			/// Raises a property changing event.
			/// </summary>
			/// <param name="propertyName">The name of the property that is changing.</param>
			void RaisePropertyChanging(string propertyName);

			/// <summary>
			/// Subscribe raise property changed events to a property changed
			/// observable. Must be called before raising property changed events.
			/// </summary>
			void SubscribePropertyChangedEvents();

			/// <summary>
			/// Raises a property changed event.
			/// </summary>
			/// <param name="propertyName">The name of the property that has changed.</param>
			void RaisePropertyChanged(string propertyName);

			/// <summary>
			/// Indicates if we are currently sending change notifications.
			/// </summary>
			/// <returns>If change notifications are being sent.</returns>
			bool AreChangeNotificationsEnabled();

			/// <summary>
			/// Suppress change notifications until the return value is disposed.
			/// </summary>
			/// <returns>A IDisposable which when disposed will re-enable change notifications.</returns>
			IDisposable SuppressChangeNotifications();

			/// <summary>
			/// Are change notifications currently delayed. Used for Observables change notifications only.
			/// </summary>
			/// <returns>If the change notifications are delayed.</returns>
			bool AreChangeNotificationsDelayed();

			/// <summary>
			/// Delay change notifications until the return value is disposed.
			/// </summary>
			/// <returns>A IDisposable which when disposed will re-enable change notifications.</returns>
			IDisposable DelayChangeNotifications();
		}

		public static void ArgumentNullExceptionThrowIfNull<T>(this T value, string name)
		{
			if (value is null)
			{
				throw new ArgumentNullException(name);
			}
		}

		public static IObservable<IReactivePropertyChangedEventArgs<TSender>> GetChangedObservable<TSender>(this TSender reactiveObject)
			where TSender : IReactiveObject
		{
			IExtensionState<IReactiveObject> val = _state.GetValue(reactiveObject, _ => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(reactiveObject));
			return val.Changed.Cast<IReactivePropertyChangedEventArgs<TSender>>();
		}

		public static IObservable<IReactivePropertyChangedEventArgs<TSender>> GetChangingObservable<TSender>(this TSender reactiveObject)
			where TSender : IReactiveObject
		{
			IExtensionState<IReactiveObject> val = _state.GetValue(reactiveObject, _ => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(reactiveObject));
			return val.Changing.Cast<IReactivePropertyChangedEventArgs<TSender>>();
		}

		public static IObservable<Exception> GetThrownExceptionsObservable<TSender>(this TSender reactiveObject)
			where TSender : IReactiveObject
		{
			IExtensionState<IReactiveObject> s = _state.GetValue(reactiveObject, _ => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(reactiveObject));
			return s.ThrownExceptions;
		}

		public static void RaisingPropertyChanging<TSender>(this TSender reactiveObject, string propertyName)
			where TSender : IReactiveObject
		{
			propertyName.ArgumentNullExceptionThrowIfNull(nameof(propertyName));
			IExtensionState<IReactiveObject> s = _state.GetValue(reactiveObject, _ => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(reactiveObject));
			s.RaisePropertyChanging(propertyName);
		}

		public static void RaisingPropertyChanged<TSender>(this TSender reactiveObject, string propertyName)
			where TSender : IReactiveObject
		{
			propertyName.ArgumentNullExceptionThrowIfNull(nameof(propertyName));
			IExtensionState<IReactiveObject> s = _state.GetValue(reactiveObject, _ => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(reactiveObject));
			s.RaisePropertyChanged(propertyName);
		}

		public static IDisposable SuppressChangeNotifications<TSender>(this TSender reactiveObject)
			where TSender : IReactiveObject
		{
			IExtensionState<IReactiveObject> s = _state.GetValue(reactiveObject, _ => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(reactiveObject));
			return s.SuppressChangeNotifications();
		}

		public static bool AreChangeNotificationsEnabled<TSender>(this TSender reactiveObject)
			where TSender : IReactiveObject
		{
			IExtensionState<IReactiveObject> s = _state.GetValue(reactiveObject, _ => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(reactiveObject));
			return s.AreChangeNotificationsEnabled();
		}

		public static IDisposable DelayChangeNotifications<TSender>(this TSender reactiveObject)
			where TSender : IReactiveObject
		{
			IExtensionState<IReactiveObject> s = _state.GetValue(reactiveObject, _ => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(reactiveObject));
			return s.DelayChangeNotifications();
		}

		private class ExtensionState<TSender> : IExtensionState<TSender>
			where TSender : IReactiveObject
		{
			private readonly Lazy<ISubject<Exception>> _thrownExceptions = new(() => new ScheduledSubject<Exception>(Scheduler.Immediate, RxApp.DefaultExceptionHandler));
			private readonly Lazy<Subject<Unit>> _startOrStopDelayingChangeNotifications = new();
			private readonly TSender _sender;
			private readonly Lazy<(ISubject<IReactivePropertyChangedEventArgs<TSender>> subject, IObservable<IReactivePropertyChangedEventArgs<TSender>> observable)> _changing;
			private readonly Lazy<(ISubject<IReactivePropertyChangedEventArgs<TSender>> subject, IObservable<IReactivePropertyChangedEventArgs<TSender>> observable)> _changed;
			private readonly Lazy<ISubject<ReactivePropertyChangingEventArgs<TSender>>> _propertyChanging;
			private readonly Lazy<ISubject<ReactivePropertyChangedEventArgs<TSender>>> _propertyChanged;

			private long _changeNotificationsSuppressed;
			private long _changeNotificationsDelayed;

			public ExtensionState(TSender sender)
			{
				_sender = sender;
				_changing = CreateLazyDelayableSubjectAndObservable();
				_changed = CreateLazyDelayableSubjectAndObservable();
				_propertyChanging = CreateLazyDelayableEventSubject<ReactivePropertyChangingEventArgs<TSender>>(_sender.RaisePropertyChanging);
				_propertyChanged = CreateLazyDelayableEventSubject<ReactivePropertyChangedEventArgs<TSender>>(_sender.RaisePropertyChanged);
			}

			public IObservable<IReactivePropertyChangedEventArgs<TSender>> Changing => _changing.Value.observable;

			public IObservable<IReactivePropertyChangedEventArgs<TSender>> Changed => _changed.Value.observable;

			public IObservable<Exception> ThrownExceptions => _thrownExceptions.Value;

			public bool AreChangeNotificationsEnabled() => Interlocked.Read(ref _changeNotificationsSuppressed) == 0;

			public bool AreChangeNotificationsDelayed() => Interlocked.Read(ref _changeNotificationsDelayed) > 0;

			public IDisposable SuppressChangeNotifications()
			{
				Interlocked.Increment(ref _changeNotificationsSuppressed);
				return Disposable.Create(() => Interlocked.Decrement(ref _changeNotificationsSuppressed));
			}

			public IDisposable DelayChangeNotifications()
			{
				if (Interlocked.Increment(ref _changeNotificationsDelayed) == 1)
				{
					if (_startOrStopDelayingChangeNotifications.IsValueCreated)
					{
						_startOrStopDelayingChangeNotifications.Value.OnNext(Unit.Default);
					}
				}

				return Disposable.Create(() =>
				{
					if (Interlocked.Decrement(ref _changeNotificationsDelayed) == 0)
					{
						if (_startOrStopDelayingChangeNotifications.IsValueCreated)
						{
							_startOrStopDelayingChangeNotifications.Value.OnNext(Unit.Default);
						}
					}
				});
			}

			public void SubscribePropertyChangingEvents() => _ = _propertyChanging.Value;

			public void RaisePropertyChanging(string propertyName)
			{
				if (!AreChangeNotificationsEnabled())
				{
					return;
				}

				ReactivePropertyChangingEventArgs<TSender> changing = new(_sender, propertyName);
				if (_propertyChanging.IsValueCreated)
				{
					_propertyChanging.Value.OnNext(changing);
				}

				if (_changing.IsValueCreated)
				{
					NotifyObservable(_sender, changing, _changing.Value.subject);
				}
			}

			public void SubscribePropertyChangedEvents() => _ = _propertyChanged.Value;

			public void RaisePropertyChanged(string propertyName)
			{
				if (!AreChangeNotificationsEnabled())
				{
					return;
				}

				ReactivePropertyChangedEventArgs<TSender> changed = new(_sender, propertyName);
				if (_propertyChanged.IsValueCreated)
				{
					_propertyChanged.Value.OnNext(changed);
				}

				if (_changed.IsValueCreated)
				{
					NotifyObservable(_sender, changed, _changed.Value.subject);
				}
			}

			private static IEnumerable<TEventArgs> DistinctEvents<TEventArgs>(IList<TEventArgs> events)
				where TEventArgs : IReactivePropertyChangedEventArgs<TSender>
			{
				if (events.Count <= 1)
				{
					return events;
				}

				HashSet<string> seen = new();
				Stack<TEventArgs> uniqueEvents = new(events.Count);

				for (int i = events.Count - 1 ; i >= 0 ; i--)
				{
					string propertyName = events[i].PropertyName;
					if (propertyName is not null && seen.Add(propertyName))
					{
						uniqueEvents.Push(events[i]);
					}
				}

				return uniqueEvents;
			}

			private void NotifyObservable<T>(TSender rxObj, T item, ISubject<T>? subject)
			{
				try
				{
					subject?.OnNext(item);
				}
				catch (Exception ex)
				{
#if INTERVENTION
					rxObj.Log().ErrorException("ReactiveObject Subscriber threw exception", ex);
#else
					rxObj.Log().Error(ex, "ReactiveObject Subscriber threw exception");
#endif
					if (!_thrownExceptions.IsValueCreated)
					{
						throw;
					}

					_thrownExceptions.Value.OnNext(ex);
				}
			}

			private Lazy<(ISubject<IReactivePropertyChangedEventArgs<TSender>> changeSubject, IObservable<IReactivePropertyChangedEventArgs<TSender>> changeObservable)> CreateLazyDelayableSubjectAndObservable() => new(() =>
			{
				Subject<IReactivePropertyChangedEventArgs<TSender>> changeSubject = new();
				IObservable<IReactivePropertyChangedEventArgs<TSender>> changeObservable = changeSubject
					.Buffer(changeSubject.Where(_ => !AreChangeNotificationsDelayed()).Select(_ => Unit.Default)
						.Merge(_startOrStopDelayingChangeNotifications.Value))
					.SelectMany(DistinctEvents)
					.Publish()
					.RefCount();

				return (changeSubject, changeObservable);
			});

			private Lazy<ISubject<TEventArgs>> CreateLazyDelayableEventSubject<TEventArgs>(Action<TEventArgs> raiseEvent) where TEventArgs : IReactivePropertyChangedEventArgs<TSender> => new(() =>
			{
				Subject<TEventArgs> changeSubject = new();
				changeSubject
					.Buffer(changeSubject.Where(_ => !AreChangeNotificationsDelayed()).Select(_ => Unit.Default)
						.Merge(_startOrStopDelayingChangeNotifications.Value))
					.SelectMany(DistinctEvents)
					.Subscribe(raiseEvent);

				return changeSubject;
			});
		}
	}
}