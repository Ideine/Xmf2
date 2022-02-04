using System;
using System.Threading.Tasks;
using MvvmCross;
using Polly;
using Xmf2.Commons.ErrorManagers;
using Xmf2.Commons.Logs;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public class BaseHttpErrorManager : IHttpErrorManager
	{
		private readonly IAsyncPolicy _httpHandlePolicy;
		private readonly ILogger _logger;

		public BaseHttpErrorManager()
		{
			_httpHandlePolicy = Policy
				.Handle<System.Net.WebException>(webEx =>
					webEx.Status == System.Net.WebExceptionStatus.ConnectFailure
					|| webEx.Status == System.Net.WebExceptionStatus.SendFailure
					|| webEx.Status == System.Net.WebExceptionStatus.UnknownError)
				.Or<Exception>(ex =>
					ex.Message.IndexOf("Bad file descriptor", StringComparison.OrdinalIgnoreCase) != -1
					|| ex.Message.IndexOf("Invalid argument", StringComparison.OrdinalIgnoreCase) != -1)
				.RetryAsync(3, (Action<Exception, int>) LogRetryException);

			Mvx.IoCProvider.TryResolve<ILogger>(out _logger);
		}

		public virtual async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
		{
			try
			{
				return
					await this.GetHttpHandlePolicy()
						.ExecuteAsync<TResult>(action)
						.ConfigureAwait(false);
			}
			catch (Exception e)
			{
				var ade = this.TreatException(e);
				this.LogAccessDataException(ade);
				throw ade;
			}
		}

		public virtual async Task ExecuteAsync(Func<Task> action)
		{
			try
			{
				await this.GetHttpHandlePolicy()
					.ExecuteAsync(action)
					.ConfigureAwait(false);
			}
			catch (Exception e)
			{
				var ade = this.TreatException(e);
				this.LogAccessDataException(ade);
				throw ade;
			}
		}

		protected virtual IAsyncPolicy GetHttpHandlePolicy()
		{
			return _httpHandlePolicy;
		}

		protected virtual AccessDataException TreatException(Exception e)
		{
			switch (e) {
				case AccessDataException ade:
					return ade;
				case OperationCanceledException oce when !oce.CancellationToken.IsCancellationRequested:
					return new AccessDataException(AccessDataException.ErrorType.Timeout);
				default:
					return new AccessDataException(AccessDataException.ErrorType.Unknown, e);
			}
		}

		protected virtual void LogRetryException(Exception e, int retryCount)
		{
			_logger?.LogWarning(e, $"HTTP retry attempt {retryCount}.");
		}

		protected virtual void LogAccessDataException(AccessDataException ade)
		{
			_logger?.LogError(ade);
			ade.IsLogged = true;
		}

		protected bool TrueInAnyInnerException(Exception e, Func<Exception, bool> testForEachInnerException)
		{
			var currentException = e;
			while (currentException != null)
			{
				if (testForEachInnerException(currentException))
					return true;
				currentException = currentException.InnerException;
			}

			return false;
		}

		protected bool TrueInAnyInnerException<TException>(Exception e, Func<TException, bool> testForEachInnerException)
			where TException : Exception
		{
			var currentException = e;
			while (currentException != null)
			{
				if (currentException is TException typedException && testForEachInnerException(typedException))
				{
					return true;
				}

				currentException = currentException.InnerException;
			}

			return false;
		}
	}
}