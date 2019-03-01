using System;
using System.Collections.Generic;

namespace Xmf2.Core.Subscriptions
{
	public class Xmf2Disposable : IDisposable
	{
		private List<IDisposable> _bindings = new List<IDisposable>();
		private List<IDisposable> _eventsDisposable = new List<IDisposable>();
		private List<IDisposable> _disposables = new List<IDisposable>();
		private List<IDisposable> _viewDisposable = new List<IDisposable>();
		private List<IDisposable> _layoutHolderDisposable = new List<IDisposable>();
		private bool _disposed;

		public void Add(IDisposable d) => _disposables.Add(d);

		public void AddView(IDisposable d) => _viewDisposable.Add(d);

		public void AddLayoutHolder(IDisposable d) => _layoutHolderDisposable.Add(d);

		public void AddBinding(IDisposable d) => _bindings.Add(d);

		public void AddEvent(IDisposable d) => _eventsDisposable.Add(d);

		~Xmf2Disposable()
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
				try
				{
					DisposeListContent(_bindings);
					DisposeListContent(_eventsDisposable);
					DisposeListContent(_disposables);
					DisposeListContent(_viewDisposable);
					DisposeListContent(_layoutHolderDisposable);

					_bindings = null;
					_eventsDisposable = null;
					_disposables = null;
					_viewDisposable = null;
					_layoutHolderDisposable = null;
				}
				catch (ObjectDisposedException) { }
			}

			_disposed = true;

			void DisposeListContent(List<IDisposable> items)
			{
				for (int i = 0; i < items.Count; i++)
				{
					items[i].Dispose();
				}

				items.Clear();
			}
		}
	}
}