using RestSharp.Portable.HttpClientImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Http;
using RestSharp.Portable;

namespace Xmf2.RestSharp.Factories
{
    public abstract class SpecificHandlerRestFactory : DefaultHttpClientFactory
    {
        private readonly System.Reflection.PropertyInfo _handlerProxyProperty;

        public SpecificHandlerRestFactory()
        {
            _handlerProxyProperty = this.GetProxyPropertyInfo();
        }

        protected override HttpMessageHandler CreateMessageHandler(IRestClient client, IRestRequest request)
        {
            var handler = this.GetMessageHandler();

            var proxy = GetProxy(client);
            if (handler.SupportsProxy && _handlerProxyProperty != null && proxy != null)
                _handlerProxyProperty.SetValue(handler, proxy, null);

            var cookieContainer = GetCookies(client, request);
            if (cookieContainer != null)
            {
                handler.UseCookies = true;
                handler.CookieContainer = cookieContainer;
            }

            var credentials = GetCredentials(request);
            if (credentials != null)
                handler.Credentials = credentials;

            //if (handler.SupportsAutomaticDecompression)
            //    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            return handler;
        }

        protected abstract HttpClientHandler GetMessageHandler();
        protected abstract PropertyInfo GetProxyPropertyInfo();
        
    }
}
