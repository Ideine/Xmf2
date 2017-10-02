using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Polly;
using Xmf2.Commons.MvxExtends.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.Droid.ErrorManagers
{
	public class DroidHttpErrorManager : BaseHttpErrorManager
	{
		Policy _droidHttpHandlePolicy;

		public DroidHttpErrorManager ()
		{
			_droidHttpHandlePolicy = Policy
                    .Handle<System.Net.WebException> (webEx =>
                        webEx.Status == System.Net.WebExceptionStatus.ConnectFailure
			|| webEx.Status == System.Net.WebExceptionStatus.SendFailure
			|| webEx.Status == System.Net.WebExceptionStatus.UnknownError
			|| webEx.Status == System.Net.WebExceptionStatus.ConnectionClosed
			|| webEx.Status == System.Net.WebExceptionStatus.KeepAliveFailure
			|| webEx.Status == System.Net.WebExceptionStatus.PipelineFailure
			|| webEx.Status == System.Net.WebExceptionStatus.ReceiveFailure
			|| webEx.Status == System.Net.WebExceptionStatus.SecureChannelFailure
			|| webEx.Status == System.Net.WebExceptionStatus.TrustFailure)
				.Or<HttpRequestException>(httpEx =>
					 httpEx.InnerException is WebException webEx && (
						  webEx.Status == System.Net.WebExceptionStatus.ConnectFailure
					   || webEx.Status == System.Net.WebExceptionStatus.SendFailure
					   || webEx.Status == System.Net.WebExceptionStatus.UnknownError
					   || webEx.Status == System.Net.WebExceptionStatus.ConnectionClosed
					   || webEx.Status == System.Net.WebExceptionStatus.KeepAliveFailure
					   || webEx.Status == System.Net.WebExceptionStatus.PipelineFailure
					   || webEx.Status == System.Net.WebExceptionStatus.ReceiveFailure
					   || webEx.Status == System.Net.WebExceptionStatus.SecureChannelFailure
					   || webEx.Status == System.Net.WebExceptionStatus.TrustFailure
						 ))
                    .Or<Exception> (ex =>
                        ex.Message.IndexOf ("Bad file descriptor", StringComparison.OrdinalIgnoreCase) != -1
			|| ex.Message.IndexOf ("Invalid argument", StringComparison.OrdinalIgnoreCase) != -1)
                    .RetryAsync (3, LogRetryException);
		}

		protected override Policy GetHttpHandlePolicy ()
		{
			return _droidHttpHandlePolicy;
		}

		protected override AccessDataException TreatException (Exception e)
		{
			var ade = e as AccessDataException;
			if (ade != null)
				return ade;

			if (e is Java.Net.UnknownHostException)
				return new AccessDataException (AccessDataException.ErrorType.NoInternetConnexion, e);

			var webEx = e as WebException;
			if (webEx == null)
			{
				webEx = (e as HttpRequestException)?.InnerException as WebException;
			}

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