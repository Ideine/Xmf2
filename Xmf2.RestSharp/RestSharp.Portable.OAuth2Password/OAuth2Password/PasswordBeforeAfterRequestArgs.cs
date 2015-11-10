using RestSharp.Portable.Authenticators.OAuth2Password.Configuration;
using System.Linq;

namespace RestSharp.Portable.Authenticators.OAuth2Password
{
    /// <summary>
    /// Event arguments used before and after a request.
    /// </summary>
    public class PasswordBeforeAfterRequestArgs
    {
        /// <summary>
        /// Client instance.
        /// </summary>
        public IRestClient Client { get; set; }

        /// <summary>
        /// Request instance.
        /// </summary>
        public IRestRequest Request { get; set; }

        /// <summary>
        /// Response instance.
        /// </summary>
        public IRestResponse Response { get; set; }

        /// <summary>
        /// Values received from service.
        /// </summary>
        public ILookup<string, string> Parameters { get; set; }

        /// <summary>
        /// Client configuration.
        /// </summary>
        public IPasswordClientConfiguration Configuration { get; set; }
    }
}