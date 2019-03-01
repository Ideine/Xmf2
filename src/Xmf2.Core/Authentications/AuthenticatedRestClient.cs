using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Core.HttpClient;
using Xmf2.Rest.HttpClient.Impl;
using RestClientExtensions = Xmf2.Rest.HttpClient.RestClientExtensions;

namespace Xmf2.Core.Authentications
{
	public interface IAuthenticatedRestClient : IRestClient
	{
		IRestClient ParentClient { get; set; }

		void Logout();
	}

	public class AuthenticatedRestClient : RestClientBase, IAuthenticatedRestClient
	{
		public AuthenticatedRestClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory ?? new DefaultHttpClientFactory())
		{
			IgnoreResponseStatusCode = true;
			Timeout = TimeSpan.FromSeconds(30);
		}

		public AuthenticatedRestClient(IHttpClientFactory httpClientFactory, Uri baseUrl) : base(httpClientFactory ?? new DefaultHttpClientFactory(), baseUrl)
		{
			IgnoreResponseStatusCode = true;
			Timeout = TimeSpan.FromSeconds(30);
		}

		public AuthenticatedRestClient(IHttpClientFactory httpClientFactory, string baseUrl) : base(httpClientFactory ?? new DefaultHttpClientFactory(), baseUrl)
		{
			IgnoreResponseStatusCode = true;
			Timeout = TimeSpan.FromSeconds(30);
		}

		protected override IHttpContent GetContent(IRestRequest request, RequestParameters parameters)
		{
			return RestClientExtensions.GetContent(this, request, parameters);
		}

		public override async Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct)
		{
			using (IHttpResponseMessage response = await ExecuteRequest(request, ct).ConfigureAwait(false))
			{
				if (response.IsSuccessStatusCode)
				{
					return await RestResponse.CreateResponse(this, request, response, ct).ConfigureAwait(false);
				}
				IRestResponse restResponse = await RestResponse.CreateResponse(this, request, response, ct).ConfigureAwait(false);
				throw new RestException(restResponse);
			}
		}

		public override async Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct)
		{
			using (IHttpResponseMessage response = await ExecuteRequest(request, ct).ConfigureAwait(false))
			{
				if (response.IsSuccessStatusCode)
				{
					return await RestResponse.CreateResponse<T>(this, request, response, ct).ConfigureAwait(false);
				}
				IRestResponse restResponse = await RestResponse.CreateResponse(this, request, response, ct).ConfigureAwait(false);
				throw new RestException(restResponse);
			}
		}

		public IRestClient ParentClient { get; set; }

		public virtual void Logout()
		{
			Authenticator = null;
		}
	}
}