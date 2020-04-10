using System.Net.Http;
using ModernHttpClient;
using Xmf2.Core.Services;
using Xmf2.Rest.HttpClient.Impl;

namespace Xmf2.Core.Droid.Services
{
	public class NativeHttpClientFactory : DefaultHttpClientFactory, INativeHttpHandlerFactory
	{
		public NativeHttpClientFactory() { }
		public NativeHttpClientFactory(bool setCredentials) : base(setCredentials) { }

		protected override HttpClientHandler NewHandler()
		{
			return new NativeMessageHandler();
		}

		HttpClientHandler INativeHttpHandlerFactory.NewHandler()
		{
			return NewHandler();
		}
	}
}