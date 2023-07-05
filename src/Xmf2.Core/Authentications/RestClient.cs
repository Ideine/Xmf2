using System;
using RestSharp.Portable;
using Xmf2.Rest.HttpClient.Impl;
using RestClientExtensions = Xmf2.Rest.HttpClient.RestClientExtensions;

namespace Xmf2.Core.Authentications
{
	public interface INoAuthRestClient : IRestClient
	{
		
	}

	public class RestClient : RestClientBase, IRestClient, INoAuthRestClient
	{
		public RestClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory ?? new DefaultHttpClientFactory())
		{
			IgnoreResponseStatusCode = true;
			Timeout = TimeSpan.FromSeconds(30);
		}

		public RestClient(IHttpClientFactory httpClientFactory, Uri baseUrl) : base(httpClientFactory ?? new DefaultHttpClientFactory(), baseUrl)
		{
			IgnoreResponseStatusCode = true;
			Timeout = TimeSpan.FromSeconds(30);
		}

		public RestClient(IHttpClientFactory httpClientFactory, string baseUrl) : base(httpClientFactory ?? new DefaultHttpClientFactory(), baseUrl)
		{
			IgnoreResponseStatusCode = true;
			Timeout = TimeSpan.FromSeconds(30);
		}

		protected override IHttpContent GetContent(IRestRequest request, RequestParameters parameters)
			=> RestClientExtensions.GetContent(this, request, parameters);
		}
}