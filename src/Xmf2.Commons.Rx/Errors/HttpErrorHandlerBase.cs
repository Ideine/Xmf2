using System;
using System.Net;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xmf2.Commons.Logs;
using Xmf2.Commons.Exceptions;
using Xmf2.Commons.Errors;
using System.Collections.Generic;

namespace Xmf2.Commons.Rx.Errors
{
	public abstract class HttpErrorHandlerBase : IHttpErrorHandler
	{
		private readonly ILogger _logger;

		private readonly Lazy<WebExceptionStatus[]> _retryStatus;
		private readonly Lazy<WebExceptionStatus[]> _timeoutStatus;
		private readonly Lazy<WebExceptionStatus[]> _noInternetStatus;

		private readonly List<string> _noInternetMessages = new List<string>
		{
			"SSL handshake timed out",
			"Unable to resolve host",
			"The operation has timed out",
			"Socket closed",
			"A task was canceled",
			"Bad file descriptor",
			"Invalid argument",
			"Software caused connection abort",
			"Failed to connect"
		};

		public HttpErrorHandlerBase(ILogger logger)
		{
			_logger = logger;

			_retryStatus = new Lazy<WebExceptionStatus[]>(RetryStatus);
			_timeoutStatus = new Lazy<WebExceptionStatus[]>(TimeoutStatus);
			_noInternetStatus = new Lazy<WebExceptionStatus[]>(NoInternetStatus);
		}

		public virtual IObservable<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
		{
			return ExecuteAsync(Observable.FromAsync(action));
		}

		public virtual IObservable<TResult> ExecuteAsync<TResult>(IObservable<TResult> source)
		{
			return ApplyRetryPolicy(source).Catch<TResult, Exception>(ex =>
			{
				AccessDataException exception = ProcessException(ex);
				LogOnException(exception);
				return Observable.Throw<TResult>(exception);
			});
		}

		protected virtual AccessDataException ProcessException(Exception ex)
		{
			switch (ex)
			{
				case AccessDataException accessData:
					return accessData;

				case OperationCanceledException operationCanceled when operationCanceled.CancellationToken.IsCancellationRequested:
					return new AccessDataException(AccessDataException.ErrorType.Timeout);

				case InvalidAppVersionException _:
					return new AccessDataException(AccessDataException.ErrorType.InvalidAppVersion, ex);

				case WebException webException when _noInternetStatus.Value.Contains(webException.Status):
					return new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex);

				case WebException webException when _timeoutStatus.Value.Contains(webException.Status):
					return new AccessDataException(AccessDataException.ErrorType.Timeout, ex);

				case TimeoutException timeoutException:
					return new AccessDataException(AccessDataException.ErrorType.Timeout, ex);

				case Rest.OAuth2.RestException restException when HttpStatusCode.NotFound == restException?.Response.StatusCode:
					return new AccessDataException(AccessDataException.ErrorType.NotFound, ex);

				case Exception exType when exType.GetType().FullName.Contains("IOException"):
					return new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex);

				default:
					return IsInternetException(ex)
						? new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex)
						: new AccessDataException(AccessDataException.ErrorType.Unknown, ex);
			}
		}

		protected virtual bool IsInternetException(Exception ex)
		{
			if (ex is AggregateException aggEx && aggEx.InnerExceptions != null)
			{
				if (aggEx.InnerExceptions.Any(IsInternetException))
				{
					return true;
				}
			}

			if (ex.InnerException != null)
			{
				if (IsInternetException(ex.InnerException))
				{
					return true;
				}
			}

			if (ex.Message != null)
			{
				return _noInternetMessages.Any(msg => ex.Message.Contains(msg));
			}

			return false;
		}

		protected virtual IObservable<TResult> ApplyRetryPolicy<TResult>(IObservable<TResult> source)
		{
			return source.WithRetryPolicy(LogOnRetry)
				.Handle<WebException>(ex => _retryStatus.Value.Contains(ex.Status))
				.Handle<Exception>(ex =>
					ex.Message.IndexOf("Bad file descriptor", StringComparison.OrdinalIgnoreCase) >= 0
					|| ex.Message.IndexOf("Invalid argument", StringComparison.OrdinalIgnoreCase) >= 0)
				.Retry(3);
		}

		protected virtual WebExceptionStatus[] RetryStatus()
		{
			return new[]
			{
				WebExceptionStatus.ConnectFailure,
				WebExceptionStatus.SendFailure,
				WebExceptionStatus.UnknownError
			};
		}

		protected virtual WebExceptionStatus[] NoInternetStatus()
		{
			return new[]
			{
				WebExceptionStatus.ConnectFailure
			};
		}

		protected virtual WebExceptionStatus[] TimeoutStatus()
		{
			return new WebExceptionStatus[]
				{ };
		}

		protected virtual void LogOnRetry(Exception ex, int attemptCount)
		{
			_logger.LogWarning(ex, $"HTTP retry attempt {attemptCount}.");
		}

		protected virtual void LogOnException(AccessDataException ex)
		{
			_logger.LogError(ex);
		}
	}
}