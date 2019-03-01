using System;
using System.Net;
using System.Threading.Tasks;
using Xmf2.Core.Exceptions;
using Xmf2.Core.HttpClient;
using System.Linq;

namespace Xmf2.Core.Errors
{
	public abstract class BaseErrorHandler : IErrorHandler
	{
		public const bool ERROR_HANDLED = true;
		public const bool ERROR_NOT_HANDLED = false;

		public async Task<bool> Handle(Exception ex, CustomErrorHandler errorHandler = null)
		{
			// first, try to use custom error handler
			if (errorHandler != null)
			{
				bool handled = await errorHandler(ex);
				if (handled)
				{
					return ERROR_HANDLED;
				}
			}
			bool exceptionHandled =    ex is NotImplementedException notImplementedException && await HandleNotImplementedException(notImplementedException)
									|| ex is InvalidAppVersionException invalidAppVersionException && await HandleInvalidAppVersionException(invalidAppVersionException)
									|| ex is AccessDataException accessDataException && await HandleAccessDataException(accessDataException)
									|| await HandleGenericException(ex);
			return exceptionHandled;
		}

		protected virtual Task<bool> HandleNotImplementedException(NotImplementedException ex) => Task.FromResult(false);
		protected virtual Task<bool> HandleInvalidAppVersionException(InvalidAppVersionException ex) => Task.FromResult(false);
		protected virtual Task<bool> HandleAccessDataException(AccessDataException ex) => Task.FromResult(false);
		protected virtual Task<bool> HandleGenericException(Exception ex) => Task.FromResult(false);

		public static bool TryGetHttpStatusCode(Exception fromEx, out HttpStatusCode errorCode)
		{
			if (TryGetRestException(fromEx, out var restException)
			   && restException.Response != null)
			{
				errorCode = restException.Response.StatusCode;
				return true;
			}
			else
			{
				errorCode = default(HttpStatusCode);
				return false;
			}
		}
		public static bool TryGetRestException(Exception fromEx, out RestException restException)
		{
			restException = fromEx.Traverse(bySelector: e => e.InnerException)
			                      .FirstOrDefault(e => e is RestException) as RestException;
			return restException != null;
		}
	}
}