using System;
using System.Reactive.Concurrency;
using System.Threading;

namespace Xmf2.Commons.Rx.Extensions
{
	//CODE FROM : https://github.com/dotnet/reactive/issues/395#issuecomment-378855644
	public sealed class ThrottleFirstObservable<T> : IObservable<T>
	{
		private readonly IObservable<T> _source;

		private readonly IScheduler _timeSource;

		private readonly TimeSpan _timespan;

		public ThrottleFirstObservable(IObservable<T> source, IScheduler timeSource, TimeSpan timespan)
		{
			_source = source;
			_timeSource = timeSource;
			_timespan = timespan;
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			var parent = new ThrottleFirstObserver(observer, _timeSource, _timespan);
			IDisposable d = _source.Subscribe(parent);
			parent.OnSubscribe(d);
			return d;
		}

		private sealed class ThrottleFirstObserver : IDisposable, IObserver<T>
		{
			private readonly IObserver<T> _downstream;

			private readonly IScheduler _timeSource;

			private readonly TimeSpan _timespan;

			private IDisposable _upstream;

			private bool _once;

			private double _due;

			internal ThrottleFirstObserver(IObserver<T> downstream,
					IScheduler timeSource, TimeSpan timespan)
			{
				_downstream = downstream;
				_timeSource = timeSource;
				_timespan = timespan;
			}

			public void OnSubscribe(IDisposable d)
			{
				if (Interlocked.CompareExchange(ref _upstream, d, null) != null)
				{
					d.Dispose();
				}
			}

			public void Dispose()
			{
				IDisposable d = Interlocked.Exchange(ref _upstream, this);
				if (d != null && d != this)
				{
					d.Dispose();
				}
			}

			public void OnCompleted()
			{
				_downstream.OnCompleted();
			}

			public void OnError(Exception error)
			{
				_downstream.OnError(error);
			}

			public void OnNext(T value)
			{
				long now = _timeSource.Now.ToUnixTimeMilliseconds();
				if (!_once)
				{
					_once = true;
					_due = now + _timespan.TotalMilliseconds;
					_downstream.OnNext(value);
				}
				else if (now >= _due)
				{
					_due = now + _timespan.TotalMilliseconds;
					_downstream.OnNext(value);
				}

			}
		}
	}
}