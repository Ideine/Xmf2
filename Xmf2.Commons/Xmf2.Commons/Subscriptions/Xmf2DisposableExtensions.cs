using System;

namespace Xmf2.Commons.Subscriptions
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

		public static TDisposable DisposeComponentWith<TDisposable>(this TDisposable disposable, Xmf2Disposable container) where TDisposable : IDisposable
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
	}
}