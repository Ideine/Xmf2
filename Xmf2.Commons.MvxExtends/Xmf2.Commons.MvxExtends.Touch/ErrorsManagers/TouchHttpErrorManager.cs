using System;
using Xmf2.Commons.MvxExtends.ErrorManagers;
using System.Net;
using Polly;
using Xmf2.Commons.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.Touch.ErrorManagers
{
	public class TouchHttpErrorManager : BaseHttpErrorManager
	{
		private readonly IAsyncPolicy _touchHttpHandlePolicy;

		public TouchHttpErrorManager()
		{
			_touchHttpHandlePolicy = Policy
				.Handle<WebException>(webEx =>
				   webEx.Status == WebExceptionStatus.ConnectFailure
				   || webEx.Status == WebExceptionStatus.SendFailure
				   || webEx.Status == WebExceptionStatus.UnknownError
				   || webEx.Status == WebExceptionStatus.ConnectionClosed
				   || webEx.Status == WebExceptionStatus.KeepAliveFailure
				   || webEx.Status == WebExceptionStatus.PipelineFailure
				   || webEx.Status == WebExceptionStatus.ReceiveFailure
				   || webEx.Status == WebExceptionStatus.SecureChannelFailure
				   || webEx.Status == WebExceptionStatus.TrustFailure)
				.Or<Exception>(ex =>
				   ex.Message.IndexOf("Bad file descriptor", StringComparison.OrdinalIgnoreCase) != -1
				   || ex.Message.IndexOf("Invalid argument", StringComparison.OrdinalIgnoreCase) != -1)
				.RetryAsync(3, LogRetryException);
		}

		protected override IAsyncPolicy GetHttpHandlePolicy()
		{
			return _touchHttpHandlePolicy;
		}

		protected override AccessDataException TreatException(Exception e)
		{
			switch (e)
			{
				case AccessDataException ade:
					return ade;
				case WebException webEx when webEx.Status == WebExceptionStatus.Timeout:
					return new AccessDataException(AccessDataException.ErrorType.Timeout, webEx);
				case WebException webEx when webEx.Status == WebExceptionStatus.ConnectFailure || webEx.Status == WebExceptionStatus.NameResolutionFailure:
					return new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, webEx);
				default:
					return base.TreatException(e);
			}
		}
	}
}

