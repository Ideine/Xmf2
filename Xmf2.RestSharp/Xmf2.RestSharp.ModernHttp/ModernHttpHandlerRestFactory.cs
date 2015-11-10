using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Http;
using Xmf2.RestSharp.Factories;

namespace Xmf2.RestSharp.ModernHttp
{
    public class ModernHttpHandlerRestFactory : SpecificHandlerRestFactory
    {
        public ModernHttpHandlerRestFactory()
            : base()
        {

        }

        protected override HttpClientHandler GetMessageHandler()
        {
            return new ModernHttpClient.NativeMessageHandler();
        }

        protected override PropertyInfo GetProxyPropertyInfo()
        {
            return typeof(ModernHttpClient.NativeMessageHandler).GetRuntimeProperty("Proxy");
        }
    }
}
