using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Xmf2.Rx
{
	public static class XmfDisposableExtensions
	{
		public static TDisposable DisposeWith<TDisposable>(this TDisposable disposable, XmfDisposable container) where TDisposable : IDisposable
		{
			container.Add(disposable);
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
	}

	public class XmfDisposable : IDisposable
	{
		private readonly CompositeDisposable _firstDisposable;
		private readonly CompositeDisposable _viewDisposable;
		private readonly CompositeDisposable _layoutHolderDisposable;
		private readonly List<IDisposable> _bindings = new List<IDisposable>();

		public XmfDisposable()
		{
			_firstDisposable = new CompositeDisposable();
			_viewDisposable = new CompositeDisposable();
			_layoutHolderDisposable = new CompositeDisposable();
		}

		public void Add(IDisposable d) => _firstDisposable.Add(d);

		public void AddView(IDisposable d) => _viewDisposable.Add(d);

		public void AddLayoutHolder(IDisposable d) => _layoutHolderDisposable.Add(d);

		public void AddBinding(IDisposable d) => _bindings.Add(d);

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					foreach(IDisposable d in _bindings)
					{
						TryDispose(d);
					}

					_firstDisposable.Dispose();
					_viewDisposable.Dispose();
					_layoutHolderDisposable.Dispose();
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
