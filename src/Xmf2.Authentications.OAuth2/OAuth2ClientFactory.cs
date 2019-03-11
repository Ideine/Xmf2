using System;
using RestSharp.Portable;
using Xmf2.Rest.HttpClient.Impl;

namespace Xmf2.Authentications.OAuth2
{
	public static class OAuth2ClientFactory
	{
		public static IOAuth2Client CreateClient(string baseUrl, OAuth2ConfigurationBase configuration, IHttpClientFactory factory = null)
		{
			return new OAuth2RestClient(factory ?? new DefaultHttpClientFactory(), baseUrl)
			{
				Configuration = configuration,
				Timeout = TimeSpan.FromSeconds(30)
			};
		}

		public static IOAuth2Client CreateClient(OAuth2ConfigurationBase configuration, IHttpClientFactory factory = null)
		{
			return new OAuth2RestClient(factory ?? new DefaultHttpClientFactory())
			{
				Configuration = configuration,
				Timeout = TimeSpan.FromSeconds(30)
			};
		}
	}
}