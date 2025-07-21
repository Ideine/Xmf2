using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Ideine.LogsSender.Extensions;
using Ideine.LogsSender.Interfaces;
using Xmf2.Commons.Errors;
using Xmf2.Commons.Exceptions;

namespace Xmf2.Rx.Errors
{
	public abstract class ErrorHandlerBase : IErrorHandler
	{
		private readonly IContextLogService _logger;

		protected ErrorHandlerBase(IContextLogService logger)
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
				default:
					return ShowErrorForException(ex, callbackAction);
			}
		}

		protected virtual bool ShowErrorForAccessDataException(AccessDataException accessDataException, Action asyncCallback)
		{
			_logger.Error($"ErrorHandler: CATCH AccessDataException {accessDataException}");
			return false;
		}

		protected virtual bool ShowErrorForManagedException(ManagedException managedException, Action asyncCallback)
		{
			_logger.Error($"ErrorHandler: CATCH ManagedException {managedException}");
			return false;
		}

		protected virtual bool ShowErrorForException(Exception exception, Action asyncCallback)
		{
			_logger.Critical($"ErrorHandler: CATCH Exception {exception}");
			return false;
		}
	}
}
