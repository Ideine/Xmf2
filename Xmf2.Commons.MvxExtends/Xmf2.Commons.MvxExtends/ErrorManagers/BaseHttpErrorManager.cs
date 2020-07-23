using Polly;
using System;
using System.Threading.Tasks;
using Xmf2.Commons.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public class BaseHttpErrorManager : IHttpErrorManager
	{
		private readonly IAsyncPolicy _httpHandlePolicy;

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

			if (e is OperationCanceledException oce)
			{
				if (oce.CancellationToken != null && !oce.CancellationToken.IsCancellationRequested)
				{
					return new AccessDataException(AccessDataException.ErrorType.Timeout);
				}
			}

			return new AccessDataException(AccessDataException.ErrorType.Unknown, e);
		}

		protected virtual void LogRetryException(Exception e, int retryCount)
		{
			System.Diagnostics.Debug.WriteLine(e, $"HTTP retry attempt {retryCount}.");
		}

		protected virtual void LogAccessDataException(AccessDataException ade)
		{
			System.Diagnostics.Debug.WriteLine(ade);
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