using System;
using System.Timers;

namespace Xmf2.Core.Helpers
{
	public class LoadingEnableHelper : IDisposable
	{
		private readonly object _mutex = new object();
		private Action<bool> _action;
		private Timer _timer;
		private bool _value;

		public LoadingEnableHelper(Action<bool> action)
		{
			_action = action;
			_timer = new Timer()
			{
				AutoReset = false,
				Interval = 200,
			};
			_timer.Elapsed += TimerOnElapsed;
		}

		private void TimerOnElapsed(object sender, ElapsedEventArgs e)
		{
			lock (_mutex)
			{
				if (_value)
				{
					_action?.Invoke(true);
				}
			}
		}

		public void Set(bool value)
		{
			lock (_mutex)
			{
				if (_value == value)
				{
					return;
				}

				_value = value;
				if (value)
				{
					_timer.Start();
				}
				else
				{
					_action?.Invoke(false);
				}
			}
		}

		public void Dispose()
		{
			_action = null;
			_timer.Elapsed -= TimerOnElapsed;
			_timer.Dispose();
			_timer = null;
		}
	}
}