using Polly;
using System;
using System.Net;
using Xmf2.Commons.ErrorManagers;
using Xmf2.Commons.MvxExtends.ErrorManagers;

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

		protected override AccessDataException TreatException(Exception e) => e switch
		{
			AccessDataException ade => ade,
			WebException webEx when webEx.Status == WebExceptionStatus.Timeout => new AccessDataException(AccessDataException.ErrorType.Timeout, webEx),
			WebException webEx when webEx.Status == WebExceptionStatus.ConnectFailure || webEx.Status == WebExceptionStatus.NameResolutionFailure => new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, webEx),
			_ => base.TreatException(e)
		};
	}
}