using System;
using System.Threading;
using RestSharp.Portable;
using Xmf2.Commons.Errors;
using Xmf2.Commons.OAuth2;

namespace Xmf2.Commons.Services
{
	public interface IRequestService
	{
		IObservable<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true);

		IObservable<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true);
	}

	public class RequestService : IRequestService
	{
		private readonly IRestClient _client;
		protected IHttpErrorHandler ErrorManager { get; }

		public RequestService(IRestClient client, IHttpErrorHandler errorManager)
		{
			_client = client;
			ErrorManager = errorManager;
		}

		public virtual IObservable<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			if (!withAuthentication)
			{
				request.AddHeader(OAuth2Authenticator.NO_AUTH_HEADER, true);
			}

			return ErrorManager.ExecuteAsync(() => _client.Execute(request, ct));
		}

		public virtual IObservable<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			if (!withAuthentication)
			{
				request.AddHeader(OAuth2Authenticator.NO_AUTH_HEADER, true);
			}

			return ErrorManager.ExecuteAsync(() => _client.Execute<T>(request, ct));
		}
	}
}
