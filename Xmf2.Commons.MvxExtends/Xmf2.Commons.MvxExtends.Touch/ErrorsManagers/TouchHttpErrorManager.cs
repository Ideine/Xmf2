using System;
using Xmf2.Commons.MvxExtends.ErrorManagers;
using System.Net;
using Polly;

namespace Xmf2.Commons.MvxExtends.Touch.ErrorManagers
{
	public class TouchHttpErrorManager: BaseHttpErrorManager
	{
		Policy _touchHttpHandlePolicy;

		public TouchHttpErrorManager ()
		{
			_touchHttpHandlePolicy = Policy
				.Handle<WebException> (webEx =>
					webEx.Status == System.Net.WebExceptionStatus.ConnectFailure
					|| webEx.Status == System.Net.WebExceptionStatus.SendFailure
					|| webEx.Status == System.Net.WebExceptionStatus.UnknownError
					|| webEx.Status == System.Net.WebExceptionStatus.ConnectionClosed
					|| webEx.Status == System.Net.WebExceptionStatus.KeepAliveFailure
					|| webEx.Status == System.Net.WebExceptionStatus.PipelineFailure
					|| webEx.Status == System.Net.WebExceptionStatus.ReceiveFailure
					|| webEx.Status == System.Net.WebExceptionStatus.SecureChannelFailure
					|| webEx.Status == System.Net.WebExceptionStatus.TrustFailure)
				.Or<Exception> (ex =>
					ex.Message.IndexOf ("Bad file descriptor", StringComparison.OrdinalIgnoreCase) != -1
			        || ex.Message.IndexOf ("Invalid argument", StringComparison.OrdinalIgnoreCase) != -1)
				.RetryAsync (3, (Action<Exception, int>)LogRetryException);
		}

		protected override Policy GetHttpHandlePolicy ()
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

