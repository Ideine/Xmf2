using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Core.Authentications;
using Xmf2.Core.Extensions;
using Xmf2.Core.Services;

namespace Xmf2.Authentications.OAuth2
{
	public class OAuth2RestClient : AuthenticatedRestClient, IOAuth2Client
	{
		private readonly SemaphoreSlim _locker = new SemaphoreSlim(1, 1);
		private OAuth2AuthResult _tokens;
		private OAuth2Authenticator _authenticator;

		public OAuth2ConfigurationBase Configuration { get; set; }

		public event EventHandler<OAuth2AuthResult> OnAuthSuccess;

		public OAuth2RestClient(IHttpClientFactory factory) : base(factory) { }

		public OAuth2RestClient(IHttpClientFactory factory, string baseUrl) : this(factory, new Uri(baseUrl)) { }

		public OAuth2RestClient(IHttpClientFactory factory, Uri baseUrl) : base(factory, baseUrl) { }

		public Task<OAuth2AuthResult> Login(string login, string password)
		{
			if (Configuration == null)
			{
				throw new InvalidOperationException("Configuration has not been set before calling login method");
			}

			IRestRequest request = new RestRequest(Configuration.LoginUrl, Configuration.LoginMethod);
			Configuration.PopulateLoginRequest(request, login, password);
			return ExecuteAuthRequest(request);
		}

		public Task<OAuth2AuthResult> Refresh()
		{
			if (Configuration == null)
			{
				throw new InvalidOperationException("Configuration has not been set before calling refresh method");
			}

			if (_tokens.RefreshToken == null)
			{
				throw new InvalidOperationException("Can not refresh without a Refresh token");
			}

			IRestRequest request = new RestRequest(Configuration.RefreshUrl, Configuration.RefreshMethod);
			Configuration.PopulateRefreshRequest(request, _tokens.RefreshToken);
			return ExecuteAuthRequest(request);
		}

		protected async Task<OAuth2AuthResult> ExecuteAuthRequest(IRestRequest request)
		{
			using (await _locker.LockAsync())
			{
				request.AddHeader(AuthenticationConstants.NO_AUTH_HEADER, true);
				IRestResponse response = await Execute(request);
				OAuth2AuthResult result = Configuration.HandleAuthResult(response);

				if (result.IsSuccess)
				{
					SetAuthenticationTokens(result);
					OnAuthSuccess?.Invoke(this, result);
				}
				else if (result.ErrorReason == AuthErrorReason.InvalidCredentials)
				{
					Logout();
				}

				return result;
			}
		}

		public override async Task Logout()
		{
			if (Configuration == null)
			{
				throw new InvalidOperationException("Configuration has not been set before calling login method");
			}

			if (_authenticator != null)
			{
				_authenticator.Access = null;
			}

			_authenticator = null;

			await base.Logout();
		}

		public void SetAuthenticationTokens(OAuth2AuthResult token)
		{
			_tokens = token;
			OAuth2Authenticator authenticator = _authenticator ?? new OAuth2Authenticator();
			authenticator.Access = token;
			Authenticator = _authenticator = authenticator;
		}
	}
}