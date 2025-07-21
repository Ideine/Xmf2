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

namespace Xmf2.Rx.Helpers
{
	public static class IReactiveObjectExtensions
	{
		private static ConditionalWeakTable<IReactiveObject, IExtensionState<IReactiveObject>> state = new ConditionalWeakTable<IReactiveObject, IExtensionState<IReactiveObject>>();

		public static IObservable<IReactivePropertyChangedEventArgs<TSender>> getChangedObservable<TSender>(this TSender This) where TSender : IReactiveObject
		{
			var val = state.GetValue(This, key => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(This));
			return val.Changed.Cast<IReactivePropertyChangedEventArgs<TSender>>();
		}

		public static IObservable<IReactivePropertyChangedEventArgs<TSender>> getChangingObservable<TSender>(this TSender This) where TSender : IReactiveObject
		{
			var val = state.GetValue(This, key => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(This));
			return val.Changing.Cast<IReactivePropertyChangedEventArgs<TSender>>();
		}
		public static IDisposable suppressChangeNotifications<TSender>(this TSender This) where TSender : IReactiveObject
		{
			var s = state.GetValue(This, key => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(This));

			return s.suppressChangeNotifications();
		}
		public static IObservable<Exception> getThrownExceptionsObservable<TSender>(this TSender This) where TSender : IReactiveObject
		{
			var s = state.GetValue(This, key => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(This));
			return s.ThrownExceptions;
		}

		public static bool areChangeNotificationsEnabled<TSender>(this TSender This) where TSender : IReactiveObject
		{
			var s = state.GetValue(This, key => (IExtensionState<IReactiveObject>)new ExtensionState<TSender>(This));

			return s.areChangeNotificationsEnabled();
		}

		// Filter a list of change notifications, returning the last change for each PropertyName in original order.
		public static IEnumerable<IReactivePropertyChangedEventArgs<TSender>> dedup<TSender>(IList<IReactivePropertyChangedEventArgs<TSender>> batch)
		{
			if (batch.Count <= 1)
			{
				return batch;
			}

			var seen = new HashSet<string>();
			var unique = new LinkedList<IReactivePropertyChangedEventArgs<TSender>>();

			for (int i = batch.Count - 1; i >= 0; i--)
			{
				if (seen.Add(batch[i].PropertyName))
				{
					unique.AddFirst(batch[i]);
				}
			}

			return unique;
		}
	}

	#region

	internal class ExtensionState<TSender> : IExtensionState<TSender> where TSender : IReactiveObject
	{
		private long changeNotificationsSuppressed;
		private long changeNotificationsDelayed;
		private ISubject<IReactivePropertyChangedEventArgs<TSender>> changingSubject;
		private IObservable<IReactivePropertyChangedEventArgs<TSender>> changingObservable;
		private ISubject<IReactivePropertyChangedEventArgs<TSender>> changedSubject;
		private IObservable<IReactivePropertyChangedEventArgs<TSender>> changedObservable;
		private ISubject<Exception> thrownExceptions;
		private ISubject<Unit> startDelayNotifications;

		private TSender sender;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionState{TSender}"/> class.
		/// </summary>
		public ExtensionState(TSender sender)
		{
			this.sender = sender;
			this.changingSubject = new Subject<IReactivePropertyChangedEventArgs<TSender>>();
			this.changedSubject = new Subject<IReactivePropertyChangedEventArgs<TSender>>();
			this.startDelayNotifications = new Subject<Unit>();
			this.thrownExceptions = new ScheduledSubject<Exception>(Scheduler.Immediate, RxApp.DefaultExceptionHandler);

			this.changedObservable = changedSubject
				.Buffer(
					Observable.Merge(
						changedSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
						startDelayNotifications)
				)
				.SelectMany(batch => IReactiveObjectExtensions.dedup(batch))
				.Publish()
				.RefCount();

			this.changingObservable = changingSubject
				.Buffer(
					Observable.Merge(
						changingSubject.Where(_ => !areChangeNotificationsDelayed()).Select(_ => Unit.Default),
						startDelayNotifications)
				)
				.SelectMany(batch => IReactiveObjectExtensions.dedup(batch))
				.Publish()
				.RefCount();
		}

		public IObservable<IReactivePropertyChangedEventArgs<TSender>> Changing => this.changingObservable;

		public IObservable<IReactivePropertyChangedEventArgs<TSender>> Changed => this.changedObservable;

		public IObservable<Exception> ThrownExceptions => thrownExceptions;

		public bool areChangeNotificationsEnabled()
		{
			return (Interlocked.Read(ref changeNotificationsSuppressed) == 0);
		}

		public bool areChangeNotificationsDelayed()
		{
			return (Interlocked.Read(ref changeNotificationsDelayed) > 0);
		}

		/// <summary>
		/// When this method is called, an object will not fire change
		/// notifications (neither traditional nor Observable notifications)
		/// until the return value is disposed.
		/// </summary>
		/// <returns>An object that, when disposed, reenables change
		/// notifications.</returns>
		public IDisposable suppressChangeNotifications()
		{
			Interlocked.Increment(ref changeNotificationsSuppressed);
			return Disposable.Create(() => Interlocked.Decrement(ref changeNotificationsSuppressed));
		}

		public IDisposable delayChangeNotifications()
		{
			if (Interlocked.Increment(ref changeNotificationsDelayed) == 1)
			{
				startDelayNotifications.OnNext(Unit.Default);
			}

			return Disposable.Create(() =>
			{
				if (Interlocked.Decrement(ref changeNotificationsDelayed) == 0)
				{
					startDelayNotifications.OnNext(Unit.Default);
				};
			});
		}

		public void raisePropertyChanging(string propertyName)
		{
			if (!this.areChangeNotificationsEnabled())
			{
				return;
			}

			var changing = new ReactivePropertyChangingEventArgs<TSender>(sender, propertyName);
			sender.RaisePropertyChanging(changing);

			this.notifyObservable(sender, changing, this.changingSubject);
		}

		public void raisePropertyChanged(string propertyName)
		{
			if (!this.areChangeNotificationsEnabled())
			{
				return;
			}

			var changed = new ReactivePropertyChangedEventArgs<TSender>(sender, propertyName);
			sender.RaisePropertyChanged(changed);

			this.notifyObservable(sender, changed, this.changedSubject);
		}

		internal void notifyObservable<T>(TSender rxObj, T item, ISubject<T> subject)
		{
			try
			{
				subject.OnNext(item);
			}
			catch (Exception ex)
			{
				rxObj.Log().ErrorException("ReactiveObject Subscriber threw exception", ex);
				thrownExceptions.OnNext(ex);
			}
		}
	}

	internal interface IExtensionState<out TSender> where TSender : IReactiveObject
	{
		IObservable<IReactivePropertyChangedEventArgs<TSender>> Changing { get; }

		IObservable<IReactivePropertyChangedEventArgs<TSender>> Changed { get; }

		void raisePropertyChanging(string propertyName);

		void raisePropertyChanged(string propertyName);

		IObservable<Exception> ThrownExceptions { get; }

		bool areChangeNotificationsEnabled();

		IDisposable suppressChangeNotifications();

		bool areChangeNotificationsDelayed();

		IDisposable delayChangeNotifications();
	}
	#endregion
}
