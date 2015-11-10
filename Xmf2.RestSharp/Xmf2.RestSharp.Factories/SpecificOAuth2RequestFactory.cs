using RestSharp.Portable;
using RestSharp.Portable.Authenticators.OAuth2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmf2.RestSharp.Factories
{
    public abstract class SpecificOAuth2RequestFactory : IRequestFactory
    {
        public IRestClient CreateClient()
        {
            var client = new RestClient();
            client.HttpClientFactory = this.GetHttpClientFactory();
            client.IgnoreResponseStatusCode = true;
            return client;
        }

        public IRestRequest CreateRequest(string resource)
        {
            return new RestRequest(resource);
        }

        protected abstract IHttpClientFactory GetHttpClientFactory();
    }
}
