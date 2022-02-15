using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;

namespace Xmf2.Rest.OAuth2
{
	public class OAuth2Authenticator : IAuthenticator
	{
		private static readonly SemaphoreSlim _refreshMutex = new SemaphoreSlim(1, 1);
		public const string NO_AUTH_HEADER = "X-No-Authenticator";

		public virtual string AuthorizationHeader { get; } = "Authorization";

		public virtual string TokenType { get; } = "Bearer";

		public OAuth2ConfigurationBase Configuration { get; set; }

		public OAuth2AuthResult Access { get; set; }

		public bool CanPreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
		{
			return Access != null && client is IOAuth2Client && !request.Parameters.Any(x => x.Name == NO_AUTH_HEADER && x.Type == ParameterType.HttpHeader);
		}

		public bool CanPreAuthenticate(IHttpClient client, IHttpRequestMessage request, ICredentials credentials)
		{
			return false;
		}

		public bool CanHandleChallenge(IHttpClient client, IHttpRequestMessage request, ICredentials credentials, IHttpResponseMessage response)
		{
			return Access != null && response.StatusCode == HttpStatusCode.Unauthorized && !request.Headers.Contains(NO_AUTH_HEADER);
		}

		public async Task PreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
		{
			string accessToken = Access?.AccessToken;
			DateTime? expireDate = Access?.ExpiresAt;

			if (accessToken == null || !expireDate.HasValue)
			{
				throw new InvalidOperationException("Missing Access data");
			}

			if (DateTime.Now.Add(Configuration.TokenSafetyMargin) > expireDate.Value)
			{
				//get a new refresh token now
				IOAuth2Client oauth2Client = client as IOAuth2Client;
				if (oauth2Client == null)
				{
					throw new NotSupportedException("This authenticator can only be used with an IOAuth2Client");
				}

				await _refreshMutex.WaitAsync();
				try
				{
					accessToken = Access?.AccessToken;
					expireDate = Access?.ExpiresAt;

					if (accessToken == null || !expireDate.HasValue)
					{
						throw new InvalidOperationException("Missing Access data");
					}

					if (DateTime.Now.Add(Configuration.TokenSafetyMargin) > expireDate.Value)
					{
						OAuth2AuthResult result = await oauth2Client.Refresh();
						if (result.IsSuccess)
						{
							accessToken = result.AccessToken;
						}
					}
				}
				finally
				{
					_refreshMutex.Release();
				}
			}

			if (accessToken == null)
			{
				throw new InvalidOperationException("Missing Access data");
			}

			request.AddHeader(AuthorizationHeader, $"{TokenType} {Access.AccessToken}");
		}

		public Task PreAuthenticate(IHttpClient client, IHttpRequestMessage request, ICredentials credentials)
		{
			throw new NotSupportedException();
		}

		public Task HandleChallenge(IHttpClient client, IHttpRequestMessage request, ICredentials credentials, IHttpResponseMessage response)
		{
			if (Access != null)
			{
				Access.ExpiresAt = DateTime.MinValue;
			}

			return Task.CompletedTask; // refresh token will be done on next request call
		}
	}
}