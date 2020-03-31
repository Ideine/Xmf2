using System;

namespace Xmf2.Commons.Subscriptions
{
	public class ActionDisposable : IDisposable
	{
		private Action _action;
		private bool _disposed;

		private ActionDisposable(Action action)
		{
			_action = action;
		}

		public static IDisposable From(Action action)
		{
			return new ActionDisposable(action);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				_action();
				_action = null;
			}

			_disposed = true;
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
}