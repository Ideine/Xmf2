using RestSharp.Portable.Authenticators.OAuth2.Configuration;
using RestSharp.Portable.Authenticators.OAuth2.Models;
using RestSharp.Portable.Authenticators.OAuth2Password.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace RestSharp.Portable.Authenticators.OAuth2Password
{
    /// <summary>
    /// Defines API for doing user authentication using certain third-party service.
    /// </summary>
    /// <remarks>
    /// Standard flow is:
    /// - client is used to generate login link (<see cref="GetLoginLinkUri"/>)
    /// - hosting app renders page with generated login link
    /// - user clicks login link - this leads to redirect to third-party service site
    /// - user authenticates and allows app access their basic information
    /// - third-party service redirects user to hosting app
    /// - hosting app reads user information using <see cref="GetUserInfo"/> method
    /// </remarks>
    public interface IPasswordClient
    {
        /// <summary>
        /// Friendly name of provider (third-party authentication service). 
        /// Defined by client implementation developer and supposed to be unique.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Obtains user information using third-party authentication service 
        /// using data provided via callback request.
        /// </summary>
        /// <param name="parameters">
        /// Callback request payload (parameters).
        /// <example>Request.QueryString</example>
        /// </param>
        Task<UserInfo> GetUserInfo(ILookup<string, string> parameters);

        /// <summary>
        /// Client configuration object.
        /// </summary>
        IPasswordClientConfiguration Configuration { get; }
    }
}