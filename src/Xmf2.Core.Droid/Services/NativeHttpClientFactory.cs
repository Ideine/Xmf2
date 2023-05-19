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
		private readonly int? _timeout;

		public NativeHttpClientFactory(int? timeout = null) : this(true, timeout) { }

		public NativeHttpClientFactory(bool setCredentials, int? timeout = null)
		{
			_setCredentials = setCredentials;
			_timeout = timeout;
		}

		protected override HttpMessageHandler CreateMessageHandler(IRestClient client)
		{
			NativeMessageHandler handler;
			if (_timeout.HasValue)
			{
				handler = new(_timeout.Value, _timeout.Value, _timeout.Value);
			}
			else
			{
				handler = new();
			}

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
			if (_timeout.HasValue)
			{
				return new NativeMessageHandler(_timeout.Value, _timeout.Value, _timeout.Value);
			}
			else
			{
				return new NativeMessageHandler();
			}
		}
	}
}