namespace RestSharp.Portable.Authenticators.OAuth2Password.Configuration
{
    /// <summary>
    /// Configuration of third-party authentication service client.
    /// </summary>
    public interface IPasswordClientConfiguration
    {
        /// <summary>
        /// Name of client type.
        /// </summary>
        string ClientTypeName { get; }

        /// <summary>
        /// Scope - contains set of permissions which user should give to your application.
        /// </summary>
        string Scope { get; }
    }
}