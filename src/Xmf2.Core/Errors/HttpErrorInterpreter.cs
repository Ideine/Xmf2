﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xmf2.Common.Extensions;
using Xmf2.Core.Exceptions;
using Xmf2.Core.HttpClient;

namespace Xmf2.Core.Errors
{
	public class HttpErrorInterpreter : IHttpErrorInterpreter
	{
		private const int STATUS_CODE_INVALID_APP_VERSION = 419;
		private const int STATUS_CODE_UPGRADE_REQUIRED = 426;

		public virtual AccessDataException InterpretException(Exception ex)
		{
			if (ex.GetType().FullName == "Java.Net.UnknownHostException"
			    || ex.GetType().Name == "SSLException")
			{
				return new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex);
			}

			if (ex.AnyToDescendant(e =>
			{
				string name = e.GetType().FullName?.ToLowerInvariant();
				//TODO: socket error code should be restricted to avoid catching exception unrelated to timeout.
				return name == "java.net.SocketTimeoutException"
				       || name == "java.net.SocketException"
				       || name == "java.net.UnknownHostException"
				       || name == "java.net.ConnectException"
				       || name == "javax.net.SSLHandshakeException"
				       || e is SocketException
				       || e is TimeoutException
				       || e is TaskCanceledException;
			}))
			{
				return new AccessDataException(AccessDataException.ErrorType.Timeout, ex);
			}

			if (ex is HttpRequestException { InnerException: WebException webException })
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
			else if (ex is HttpRequestException httpException && httpException.Message.Contains("SSL"))
			{
				return new AccessDataException(AccessDataException.ErrorType.Timeout, ex);
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

				case RestException restEx when (int)restEx.Response.StatusCode == STATUS_CODE_INVALID_APP_VERSION
				                               || (int)restEx.Response.StatusCode == STATUS_CODE_UPGRADE_REQUIRED:
					return new AccessDataException(AccessDataException.ErrorType.InvalidAppVersion, restEx);

				case RestException restEx when restEx.Response.StatusCode == HttpStatusCode.BadGateway:
				case OperationCanceledException { CancellationToken: { IsCancellationRequested: true } }:
					return new AccessDataException(AccessDataException.ErrorType.Timeout);

				case WebException { Status: WebExceptionStatus.ConnectFailure }:
					return new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex);

				default:
					return new AccessDataException(AccessDataException.ErrorType.Unknown, ex);
			}
		}
	}
}