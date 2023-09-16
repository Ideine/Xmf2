using System;
using System.Net;
using Java.Net;
using Polly;
using Xmf2.Commons.MvxExtends.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.Droid.ErrorManagers;

public class DroidHttpErrorManager : BaseHttpErrorManager
{
	private readonly IAsyncPolicy _droidHttpHandlePolicy;

	public DroidHttpErrorManager()
	{
		_droidHttpHandlePolicy = Policy
			.Handle<WebException>(webEx =>
				webEx.Status is WebExceptionStatus.ConnectFailure
					or WebExceptionStatus.SendFailure
					or WebExceptionStatus.UnknownError
					or WebExceptionStatus.ConnectionClosed
					or WebExceptionStatus.KeepAliveFailure
					or WebExceptionStatus.PipelineFailure
					or WebExceptionStatus.ReceiveFailure
					or WebExceptionStatus.SecureChannelFailure
					or WebExceptionStatus.TrustFailure)
			.Or<Exception>(ex =>
				ex.Message.IndexOf("Bad file descriptor", StringComparison.OrdinalIgnoreCase) != -1
				|| ex.Message.IndexOf("Invalid argument", StringComparison.OrdinalIgnoreCase) != -1)
			.RetryAsync(3, LogRetryException);
	}

	protected override IAsyncPolicy GetHttpHandlePolicy()
	{
		return _droidHttpHandlePolicy;
	}

	protected override AccessDataException TreatException(Exception ex) => ex switch
	{
		AccessDataException ade => ade,
		UnknownHostException => new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, ex),
		WebException { Status: WebExceptionStatus.Timeout } webEx => new AccessDataException(AccessDataException.ErrorType.Timeout, webEx),
		WebException { Status: WebExceptionStatus.ConnectFailure } webEx => new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, webEx),
		WebException { Status: WebExceptionStatus.NameResolutionFailure } webEx => new AccessDataException(AccessDataException.ErrorType.NoInternetConnexion, webEx),
		_ => base.TreatException(ex)
	};
}