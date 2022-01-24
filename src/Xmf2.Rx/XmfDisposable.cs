﻿using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;

namespace Xmf2.Rx
{
	public static class XmfDisposableExtensions
	{
		public static TDisposable DisposeWith<TDisposable>(this TDisposable disposable, XmfDisposable container) where TDisposable : IDisposable
		{
			container.Add(disposable);
			return disposable;
		}

		public static TDisposable DisposeEvent<TDisposable>(this TDisposable disposable, XmfDisposable container) where TDisposable : IDisposable
		{
			container.AddEvent(disposable);
			return disposable;
		}

		public static TDisposable DisposeBinding<TDisposable>(this TDisposable disposable, XmfDisposable container) where TDisposable : IDisposable
		{
			container.AddBinding(disposable);
			return disposable;
		}

		public static TDisposable DisposeView<TDisposable>(this TDisposable disposable, XmfDisposable container) where TDisposable : IDisposable
		{
			container.AddView(disposable);
			return disposable;
		}

		public static TDisposable DisposeLayoutHolder<TDisposable>(this TDisposable disposable, XmfDisposable container) where TDisposable : IDisposable
		{
			container.AddLayoutHolder(disposable);
			return disposable;
		}

		public static XmfDisposable RegisterAndDisposeListeners<TListener>(this XmfDisposable container, Action<TListener> setListener, TListener value) where TListener : class
		{
			setListener(value);
			ActionDisposable.From(() => setListener(null)).DisposeEvent(container);
			return container;
		}
	}

	public class ActionDisposable : IDisposable
	{
		private Action _action;

		private ActionDisposable(Action action)
		{
			_action = action;
		}

		public static IDisposable From(Action action)
		{
			return new ActionDisposable(action);
		}

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_action();
					_action = null;
				}

				disposedValue = true;
			}
		}

		~ActionDisposable()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}

	public class XmfDisposable : IDisposable
	{
		private readonly List<IDisposable> _bindings = new List<IDisposable>();
		private readonly CompositeDisposable _eventsDisposable = new CompositeDisposable();
		private readonly CompositeDisposable _firstDisposable = new CompositeDisposable();
		private readonly CompositeDisposable _viewDisposable = new CompositeDisposable();
		private readonly CompositeDisposable _layoutHolderDisposable = new CompositeDisposable();

		public void Add(IDisposable d) => _firstDisposable.Add(d);

		public void AddView(IDisposable d) => _viewDisposable.Add(d);

		public void AddLayoutHolder(IDisposable d) => _layoutHolderDisposable.Add(d);

		public void AddBinding(IDisposable d) => _bindings.Add(d);

		public void AddEvent(IDisposable d) => _eventsDisposable.Add(d);

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					foreach (IDisposable d in _bindings)
					{
						TryDispose(d);
					}

					_eventsDisposable.Dispose();

					Task.Run(async () =>
					{
						//Wait for binding & event finish disposing
						await Task.Delay(TimeSpan.FromSeconds(10));
						RxApp.MainThreadScheduler.Schedule(() =>
						{
							_firstDisposable.Dispose();
							_viewDisposable.Dispose();
							_layoutHolderDisposable.Dispose();
						});
					});

				}

				disposedValue = true;
			}
		}

		~XmfDisposable()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public static bool TryDispose(IDisposable obj)
		{
			try
			{
				if (obj != null)
				{
					obj?.Dispose();
					return true;
				}
			}
			catch (ObjectDisposedException)
			{
			}

			return false;
		}
	}
}
