using System;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels
{
	public class Busy : IBusy, IDisposable
	{
		private IStateRaiser _raiser;
		private readonly object _lock = new object();
		private int _count;

		public Busy(IStateRaiser raiser)
		{
			_raiser = raiser;
		}

		public bool IsEnabled => _count > 0;

		public void Inc()
		{
			bool notify;
			lock (_lock)
			{
				notify = _count == 0;
				_count++;
			}

			if (notify)
			{
				_raiser?.RaiseStateChanged();
			}
		}

		public void Dec()
		{
			bool notify;
			lock (_lock)
			{
				notify = _count == 1;
				_count--;
				if (_count < 0)
				{
					_count = 0;
				}
			}

			if (notify)
			{
				_raiser?.RaiseStateChanged();
			}
		}

		public IDisposable EnableDisposable() => new IncDisposable(this);

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_raiser = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Busy()
		{
			Dispose(false);
		}

		#endregion

		private class IncDisposable : IDisposable
		{
			private IBusy _busy;

			public IncDisposable(IBusy busy)
			{
				_busy = busy;
				_busy.Inc();
			}

			public void Dispose()
			{
				_busy.Dec();
				_busy = null;
			}
		}
	}
}