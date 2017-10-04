using System;
using System.Reactive.Linq;

namespace Xmf2.Rx.Extensions
{
	public static class RetriesExtensions
	{
		public interface IRetryObservableBuilder<out TResult>
		{
			IRetryObservableBuilder<TResult> Handle<TException>(Func<TException, bool> handler) where TException : Exception;

			IObservable<TResult> Retry(int retryCount);
		}

		public static IRetryObservableBuilder<TResult> WithRetryPolicy<TResult>(this IObservable<TResult> observable, Action<Exception, int> loggerCall = null)
		{
			var builder = new RetryObservableBuilder<TResult>(observable, loggerCall);

			return builder;
		}

		private class RetryObservableBuilder<TResult> : IRetryObservableBuilder<TResult>
		{
			private class RetryResult
			{
				public bool IsSuccess { get; }

				public TResult Result { get; }

				public Exception Exception { get; }

				private RetryResult(bool isSuccess, TResult result, Exception exception)
				{
					IsSuccess = isSuccess;
					Result = result;
					Exception = exception;
				}

				public static RetryResult FromResult(TResult result)
				{
					return new RetryResult(true, result, null);
				}

				public static RetryResult FromException(Exception ex)
				{
					return new RetryResult(false, default(TResult), ex);
				}
			}

			private readonly Action<Exception, int> _loggerCall;
			private IObservable<RetryResult> _observable;
			private int _attemptCount;

			public RetryObservableBuilder(IObservable<TResult> observable, Action<Exception, int> loggerCall = null)
			{
				_observable = observable.Select(RetryResult.FromResult);
				_loggerCall = loggerCall;
				_attemptCount = 0;
			}

			public IRetryObservableBuilder<TResult> Handle<TException>(Func<TException, bool> handler) where TException : Exception
			{
				_observable = _observable.Catch<RetryResult, TException>(ex =>
				{
					if (handler(ex))
					{
						if(_attemptCount != 0)
						{
							_loggerCall?.Invoke(ex, _attemptCount);
						}
						_attemptCount++;
						return Observable.Throw<RetryResult>(ex);
					}
					return Observable.Return(RetryResult.FromException(ex));
				});

				return this;
			}

			public IObservable<TResult> Retry(int retryCount)
			{
				return _observable.Retry(retryCount)
					.SelectMany(retryResult => retryResult.IsSuccess ? Observable.Return(retryResult.Result) : Observable.Throw<TResult>(retryResult.Exception))
					.Select(x => 
					{
						_attemptCount = 0;
						return x;
					}).Catch<TResult, Exception>(ex => 
					{
						_attemptCount = 0;
						return Observable.Throw<TResult>(ex);
					});
			}
		}
	}
}
