using System.Net;
using System.Net.Http;
using ModernHttpClient;
using RestSharp.Portable;
using Xmf2.Core.Services;
using Xmf2.Rest.HttpClient.Impl;

namespace Xmf2.Core.Droid.Services
{
	public class NativeHttpClientFactory : DefaultHttpClientFactory, INativeHttpHandlerFactory
	{
		private readonly bool _setCredentials;

		public NativeHttpClientFactory() : this(true) { }

		public NativeHttpClientFactory(bool setCredentials)
		{
			_setCredentials = setCredentials;
		}

		protected override HttpMessageHandler CreateMessageHandler(IRestClient client)
		{
			var handler = new NativeMessageHandler();

#if !NO_PROXY
			if (handler.SupportsProxy && client.Proxy != null)
			{
				handler.Proxy = client.Proxy;
			}
#endif

			if (client.CookieContainer != null)
			{
				handler.UseCookies = true;
				handler.CookieContainer = client.CookieContainer;
			}

			if (_setCredentials)
			{
				ICredentials credentials = client.Credentials;
				if (credentials != null)
				{
					handler.Credentials = credentials;
				}
			}

			if (handler.SupportsAutomaticDecompression)
			{
				handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			}

			return handler;
		}

		HttpClientHandler INativeHttpHandlerFactory.NewHandler()
		{
			return new NativeMessageHandler();
		}
	}
}