﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xmf2.Core.Exceptions;
using Xmf2.Core.HttpClient;

namespace Xmf2.Core.Errors
{
	public class HttpErrorInterpreter : IHttpErrorInterpreter
	{
		private const int STATUS_CODE_INVALID_APP_VERSION = 419;

		public virtual AccessDataException InterpretException(Exception ex)
		{
			if (ex.GetType().FullName == "Java.Net.UnknownHostException"
				|| ex.GetType().Name == "SSLException")
			{
				return new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex);
			}

			if (AnyToDescendant(ex, e =>
			{
				string name = e.GetType().FullName?.ToLowerInvariant();

				return name == "java.net.SocketTimeoutException" ||
					   name == "java.net.SocketException" ||
					   name == "java.net.ConnectException" ||
					   name == "javax.net.SSLHandshakeException" ||
					   e is SocketException ||
					   e is TimeoutException ||
					   e is TaskCanceledException;
			}))
			{
				return new AccessDataException(AccessDataException.ErrorType.Timeout, ex);
			}

			if (ex is HttpRequestException httpException && httpException.InnerException is WebException webException)
			{
				switch (webException.Status)
				{
					case WebExceptionStatus.ConnectFailure:
					case WebExceptionStatus.NameResolutionFailure:
						return new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex);
					case WebExceptionStatus.Timeout:
						return new AccessDataException(AccessDataException.ErrorType.Timeout, ex);
				}
			}

			switch (ex)
			{
				case AccessDataException accessDataException:
					return accessDataException;

				case RestException restEx when restEx.Response.StatusCode == HttpStatusCode.NotFound:
					return new AccessDataException(AccessDataException.ErrorType.NotFound, ex);

				case RestException restEx when restEx.Response.StatusCode == HttpStatusCode.Forbidden:
					return new AccessDataException(AccessDataException.ErrorType.Forbidden, ex);

				case RestException restEx when restEx.Response.StatusCode == HttpStatusCode.Unauthorized:
					return new AccessDataException(AccessDataException.ErrorType.UnAuthorized, ex);

				case RestException restEx when (int)restEx.Response.StatusCode == STATUS_CODE_INVALID_APP_VERSION:
					return new AccessDataException(AccessDataException.ErrorType.InvalidAppVersion, restEx);

				case OperationCanceledException canceledException when canceledException.CancellationToken.IsCancellationRequested:
					return new AccessDataException(AccessDataException.ErrorType.Timeout);

				case WebException webEx when webEx.Status == WebExceptionStatus.ConnectFailure:
					return new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex);

				default:
					return new AccessDataException(AccessDataException.ErrorType.Unknown, ex);
			}
		}

		#region Helper

		private static bool AnyToDescendant(Exception baseEx, Func<Exception, bool> action)
		{
			var ex = baseEx;

			while (ex != null)
			{
				if (action(ex))
				{
					return true;
				}

				ex = ex.InnerException;
			}

			return false;
		}

		#endregion
	}
}