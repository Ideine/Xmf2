//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using RestSharp.Portable;
//using Xmf2.Common.Extensions;
//using Xmf2.Core.Authentications;
//using Xmf2.Core.Extensions;
//using Xmf2.Core.Services;

//namespace Xmf2.Authentications.OAuth2
//{
//	public class OAuth2RestClient : AuthenticatedRestClient, IOAuth2Client
//	{
//		private readonly SemaphoreSlim _locker = new SemaphoreSlim(1, 1);
//		private OAuth2AuthResult _tokens;
//		private OAuth2Authenticator _authenticator;

//		public OAuth2ConfigurationBase Configuration { get; set; }

//		public event EventHandler<OAuth2AuthResult> OnAuthSuccess;

//		public OAuth2RestClient(IHttpClientFactory factory) : base(factory) { }

//		public OAuth2RestClient(IHttpClientFactory factory, string baseUrl) : this(factory, new Uri(baseUrl)) { }

//		public OAuth2RestClient(IHttpClientFactory factory, Uri baseUrl) : base(factory, baseUrl) { }

//		public async Task<OAuth2AuthResult> Login(string login, string password)
//		{
//			using (await _locker.LockAsync())
//			{
//				if (Configuration == null)
//				{
//					throw new InvalidOperationException("Configuration has not been set before calling login method");
//				}

//				IRestRequest RequestFunc()
//				{
//					IRestRequest request = new RestRequest(Configuration.LoginUrl, Configuration.LoginMethod);
//					Configuration.PopulateLoginRequest(request, login, password);
//					return request;
//				}

//				return await ExecuteAuthRequest(RequestFunc);
//			}
//		}

//		public async Task<OAuth2AuthResult> Refresh()
//		{
//			using (await _locker.LockAsync())
//			{
//				if (Configuration == null)
//				{
//					throw new InvalidOperationException("Configuration has not been set before calling refresh method");
//				}

//				if (_tokens.RefreshToken == null)
//				{
//					throw new InvalidOperationException("Can not refresh without a Refresh token");
//				}

//				IRestRequest RequestFunc()
//				{
//					var request = new RestRequest(Configuration.RefreshUrl, Configuration.RefreshMethod);
//					Configuration.PopulateRefreshRequest(request, _tokens.RefreshToken);
//					return request;
//				}

//				return await ExecuteAuthRequest(RequestFunc);
//			}
//		}

//		protected async Task<OAuth2AuthResult> ExecuteAuthRequest(Func<IRestRequest> requestFunc)
//		{
//			IRestRequest request = requestFunc.Invoke();
//			request.AddHeader(AuthenticationConstants.NO_AUTH_HEADER, true);
//			IRestResponse response = await Execute(request);
//			OAuth2AuthResult result = Configuration.HandleAuthResult(response);

//			if (result.IsSuccess)
//			{
//				SetAuthenticationTokens(result);
//				OnAuthSuccess?.Invoke(this, result);
//			}
//			else if (result.ErrorReason == AuthErrorReason.InvalidCredentials)
//			{
//				Logout();
//			}

//			return result;
//		}

//		public override async Task Logout()
//		{
//			if (Configuration == null)
//			{
//				throw new InvalidOperationException("Configuration has not been set before calling login method");
//			}

//			if (_authenticator != null)
//			{
//				_authenticator.Access = null;
//			}

//			_authenticator = null;

//			await base.Logout();
//		}

//		protected virtual OAuth2Authenticator CreateAuthenticator()
//		{
//			return new OAuth2Authenticator();
//		}

//		public void SetAuthenticationTokens(OAuth2AuthResult token)
//		{
//			_tokens = token;
//			OAuth2Authenticator authenticator = _authenticator ?? CreateAuthenticator();
//			authenticator.Access = token;
//			Authenticator = _authenticator = authenticator;
//		}
//	}
//}