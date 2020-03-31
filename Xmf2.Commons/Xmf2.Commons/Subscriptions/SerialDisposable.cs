using System;

namespace Xmf2.Commons.Subscriptions
{
	public class SerialDisposable : IDisposable
	{
		private IDisposable _disposable;

		public IDisposable Disposable
		{
			get => _disposable;
			set
			{
				_disposable?.Dispose();
				_disposable = value;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable?.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~SerialDisposable()
		{
			Dispose(false);
		}
	}
}