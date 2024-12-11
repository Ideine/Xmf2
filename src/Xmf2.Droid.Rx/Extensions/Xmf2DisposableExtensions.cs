using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;

namespace Xmf2.Droid.Rx.Extensions
{
	public static class Xmf2DisposableExtensions
	{
		public static TDisposable DisposeWith<TDisposable>(this TDisposable disposable, Xmf2Disposable container) where TDisposable : IDisposable
		{
			container.Add(disposable);
			return disposable;
		}

		public static TDisposable DisposeEventWith<TDisposable>(this TDisposable disposable, Xmf2Disposable container) where TDisposable : IDisposable
		{
			container.AddEvent(disposable);
			return disposable;
		}

		public static TDisposable DisposeBindingWith<TDisposable>(this TDisposable disposable, Xmf2Disposable container) where TDisposable : IDisposable
		{
			container.AddBinding(disposable);
			return disposable;
		}

		public static TDisposable DisposeViewWith<TDisposable>(this TDisposable disposable, Xmf2Disposable container) where TDisposable : IDisposable
		{
			container.AddView(disposable);
			return disposable;
		}

		public static TDisposable DisposeLayoutHolderWith<TDisposable>(this TDisposable disposable, Xmf2Disposable container) where TDisposable : IDisposable
		{
			container.AddLayoutHolder(disposable);
			return disposable;
		}

		public static Xmf2Disposable RegisterAndDisposeListeners<TListener>(this Xmf2Disposable container, Action<TListener> setListener, TListener value) where TListener : class
		{
			setListener(value);
			ActionDisposable.From(() => setListener(null)).DisposeEventWith(container);
			return container;
		}

		public static IDisposable WhenActivatedAndDispose(this IActivatableView @this, Xmf2Disposable disposables, Action<CompositeDisposable> block, IViewFor view = null)
		{
			return @this.WhenActivated(dispo =>
			{
				dispo.DisposeBindingWith(disposables);
				block(dispo);
			}, view);
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

		private bool _disposedValue; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_action();
					_action = null;
				}

				_disposedValue = true;
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

	public class Xmf2Disposable : IDisposable
	{
		private readonly List<IDisposable> _bindings = new();
		private readonly CompositeDisposable _eventsDisposable = new();
		private readonly CompositeDisposable _firstDisposable = new();
		private readonly CompositeDisposable _viewDisposable = new();
		private readonly CompositeDisposable _layoutHolderDisposable = new();

		public void Add(IDisposable d) => _firstDisposable.Add(d);

		public void AddView(IDisposable d) => _viewDisposable.Add(d);

		public void AddLayoutHolder(IDisposable d) => _layoutHolderDisposable.Add(d);

		public void AddBinding(IDisposable d) => _bindings.Add(d);

		public void AddEvent(IDisposable d) => _eventsDisposable.Add(d);

		private bool _disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
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
							TryDispose(_firstDisposable);
							TryDispose(_viewDisposable);
							TryDispose(_layoutHolderDisposable);
						});
					});
				}

				_disposedValue = true;
			}
		}

		~Xmf2Disposable()
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
			catch (ObjectDisposedException) { }

			return false;
		}
	}
}