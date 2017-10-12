using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xmf2.Commons.Errors;
using Xmf2.Commons.Exceptions;
using Xmf2.Commons.Logs;

namespace Xmf2.Rx.Errors
{
	public abstract class ErrorHandlerBase : IErrorHandler
	{
		private readonly ILogger _logger;

		protected ErrorHandlerBase(ILogger logger)
		{
			_logger = logger;
		}

		public IObservable<TResult> Execute<TResult>(Func<TResult> action, CustomErrorHandler errorHandler = null)
		{
			return ExecuteAsync(() => Task.FromResult(action()), errorHandler);
		}

		public IObservable<TResult> Execute<TResult>(IObservable<TResult> source, CustomErrorHandler errorHandler = null)
		{
			return WrapForError(source, errorHandler);
		}

		public IObservable<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action, CustomErrorHandler errorHandler = null)
		{
			return Execute(Observable.FromAsync(action), errorHandler);
		}

		protected IObservable<TResult> WrapForError<TResult>(IObservable<TResult> source, CustomErrorHandler errorHandler)
		{
			return source.Catch<TResult, Exception>(ex =>
			{
				return Observable.FromAsync<TResult>(async () =>
				{
					await HandleError(ex, errorHandler);
					throw ex;
				});
			});
		}

		public Task HandleError(Exception ex, CustomErrorHandler errorHandler = null)
		{
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

			bool customErrorHandled = errorHandler?.Invoke(ex, () => tcs.TrySetResult(true)) ?? false;

			if (customErrorHandled)
			{
				return tcs.Task;
			}

			bool genericErrorHandled = HandleGenericError(ex, () => tcs.TrySetResult(true));

			if (genericErrorHandled)
			{
				return tcs.Task;
			}

			tcs.TrySetResult(true);
			return tcs.Task;
		}

		protected virtual bool HandleGenericError(Exception ex, Action callbackAction)
		{
			switch (ex)
			{
				case AccessDataException accessData:
					return ShowErrorForAccessDataException(accessData, callbackAction);
				case ManagedException managed:
					return ShowErrorForManagedException(managed, callbackAction);
			}

			return ShowErrorForException(ex, callbackAction);
		}

		protected virtual bool ShowErrorForAccessDataException(AccessDataException accessDataException, Action asyncCallback)
		{
			_logger.LogError(accessDataException, "ErrorHandler: CATCH AccessDataException");
			return false;
		}

		protected virtual bool ShowErrorForManagedException(ManagedException managedException, Action asyncCallback)
		{
			_logger.LogError(managedException, "ErrorHandler: CATCH ManagedException");
			return false;
		}

		protected virtual bool ShowErrorForException(Exception exception, Action asyncCallback)
		{
			_logger.LogCritical(exception, "ErrorHandler: CATCH Exception");
			return false;
		}
	}
}
