namespace RestSharp.Portable.Authenticators.OAuth2Password.Configuration
{
    /// <summary>
    /// Runtime client configuration. 
    /// </summary>
    /// <remarks>
    /// This is a small in-memory implementation of <see cref="IClientConfiguration"/>
    /// </remarks>
    public class RuntimePasswordClientConfiguration : IPasswordClientConfiguration
    {
        /// <summary>
        /// Name of client type.
        /// </summary>
        public string ClientTypeName { get; set; }

        /// <summary>
        /// Client ID (ID of your application).
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Scope - contains set of permissions which user should give to your application.
        /// </summary>
        public string Scope { get; set; }
    }
}