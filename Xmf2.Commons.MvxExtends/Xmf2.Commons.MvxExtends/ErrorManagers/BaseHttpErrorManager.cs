using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Polly;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public class BaseHttpErrorManager : IHttpErrorManager
	{
		private readonly IAsyncPolicy _httpHandlePolicy;

		public BaseHttpErrorManager()
		{
			_httpHandlePolicy = Policy
				.Handle<WebException>(webEx =>
					webEx.Status == WebExceptionStatus.ConnectFailure
					|| webEx.Status == WebExceptionStatus.SendFailure
					|| webEx.Status == WebExceptionStatus.UnknownError)
				.Or<Exception>(ex =>
					ex.Message.IndexOf("Bad file descriptor", StringComparison.OrdinalIgnoreCase) != -1
					|| ex.Message.IndexOf("Invalid argument", StringComparison.OrdinalIgnoreCase) != -1)
				.RetryAsync(3, LogRetryException);
		}

		public virtual async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
		{
			try
			{
				return
					await GetHttpHandlePolicy()
						.ExecuteAsync(action)
						.ConfigureAwait(false);
			}
			catch (Exception e)
			{
				AccessDataException ade = TreatException(e);
				LogAccessDataException(ade);
				throw ade;
			}
		}

		public virtual async Task ExecuteAsync(Func<Task> action)
		{
			try
			{
				await GetHttpHandlePolicy()
					.ExecuteAsync(action)
					.ConfigureAwait(false);
			}
			catch (Exception e)
			{
				AccessDataException ade = TreatException(e);
				LogAccessDataException(ade);
				throw ade;
			}
		}

		protected virtual IAsyncPolicy GetHttpHandlePolicy()
		{
			return _httpHandlePolicy;
		}

		protected virtual AccessDataException TreatException(Exception e)
		{
			if (e is AccessDataException ade)
			{
				return ade;
			}

			if (e is OperationCanceledException { CancellationToken.IsCancellationRequested: false })
			{
				return new AccessDataException(AccessDataException.ErrorType.Timeout);
			}

			return new AccessDataException(AccessDataException.ErrorType.Unknown, e);
		}

		protected virtual void LogRetryException(Exception e, int retryCount)
		{
			Debug.WriteLine(e, $"HTTP retry attempt {retryCount}.");
		}

		protected virtual void LogAccessDataException(AccessDataException ade)
		{
			Debug.WriteLine(ade);
			ade.IsLogged = true;
		}

		protected bool TrueInAnyInnerException(Exception e, Func<Exception, bool> testForEachInnerException)
		{
			Exception currentException = e;
			while (currentException != null)
			{
				if (testForEachInnerException(currentException))
				{
					return true;
				}

				currentException = currentException.InnerException;
			}

			return false;
		}

		protected bool TrueInAnyInnerException<TException>(Exception e, Func<TException, bool> testForEachInnerException)
			where TException : Exception
		{
			Exception currentException = e;
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