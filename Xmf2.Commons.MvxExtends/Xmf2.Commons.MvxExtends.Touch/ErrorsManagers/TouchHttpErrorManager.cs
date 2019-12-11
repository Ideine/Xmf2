using System;
using Xmf2.Commons.MvxExtends.ErrorManagers;
using System.Net;
using Polly;
using Xmf2.Commons.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.Touch.ErrorManagers
{
	public class TouchHttpErrorManager: BaseHttpErrorManager
	{
		IAsyncPolicy _touchHttpHandlePolicy;

		public TouchHttpErrorManager ()
		{
			_touchHttpHandlePolicy = Policy
				.Handle<WebException> (webEx =>
					webEx.Status == WebExceptionStatus.ConnectFailure
					|| webEx.Status == WebExceptionStatus.SendFailure
					|| webEx.Status == WebExceptionStatus.UnknownError
					|| webEx.Status == WebExceptionStatus.ConnectionClosed
					|| webEx.Status == WebExceptionStatus.KeepAliveFailure
					|| webEx.Status == WebExceptionStatus.PipelineFailure
					|| webEx.Status == WebExceptionStatus.ReceiveFailure
					|| webEx.Status == WebExceptionStatus.SecureChannelFailure
					|| webEx.Status == WebExceptionStatus.TrustFailure)
				.Or<Exception> (ex =>
					ex.Message.IndexOf ("Bad file descriptor", StringComparison.OrdinalIgnoreCase) != -1
			        || ex.Message.IndexOf ("Invalid argument", StringComparison.OrdinalIgnoreCase) != -1)
				.RetryAsync (3, (Action<Exception, int>)LogRetryException);
		}

		protected override IAsyncPolicy GetHttpHandlePolicy ()
		{
			return _touchHttpHandlePolicy;
		}

		protected override AccessDataException TreatException (Exception e)
		{
			var ade = e as AccessDataException;
			if (ade != null)
				return ade;

            var webEx = e as WebException;
			if (webEx != null) {
				switch (webEx.Status) {
				case WebExceptionStatus.Timeout:
					return new AccessDataException (AccessDataException.ErrorType.Timeout, webEx);
				case WebExceptionStatus.ConnectFailure:
					return new AccessDataException (AccessDataException.ErrorType.NoInternetConnexion, webEx);
				case WebExceptionStatus.NameResolutionFailure:
					return new AccessDataException (AccessDataException.ErrorType.NoInternetConnexion, webEx);
				}                 
			}
			return base.TreatException (e);
		}
	}
}

