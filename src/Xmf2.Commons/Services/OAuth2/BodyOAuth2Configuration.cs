using RestSharp.Portable;
using Xmf2.Commons.Services.OAuth2.Models;
using Xmf2.Rest.OAuth2;

namespace Xmf2.Commons.Services.OAuth2
{
	public class OAuth2BodyConfiguration : OAuth2ConfigurationBase<AuthenticationResponse>
	{
		private readonly string _clientId;
		private readonly string _clientSecret;

		public OAuth2BodyConfiguration(string clientId, string clientSecret)
		{
			_clientId = clientId;
			_clientSecret = clientSecret;
			LoginUrl = "token";
			RefreshUrl = "token";
			LoginMethod = Method.POST;
			RefreshMethod = Method.POST;
		}

		public OAuth2BodyConfiguration(string clientId, string clientSecret, string loginUrl, string refreshUrl) : this(clientId, clientSecret)
		{
			LoginUrl = loginUrl;
			RefreshUrl = refreshUrl;
		}

		public OAuth2BodyConfiguration(string clientId, string clientSecret, Method loginMethod, string loginUrl, Method refreshMethod, string refreshUrl) : this(clientId, clientSecret, loginUrl, refreshUrl)
		{
			LoginMethod = loginMethod;
			RefreshMethod = refreshMethod;
		}

		public override void PopulateLoginRequest(IRestRequest request, string login, string password)
		{
			request.AddBody(new LoginRequest
			{
				ClientId = _clientId,
				ClientSecret = _clientSecret,
				Login = login,
				Password = password
			});
		}

		public override void PopulateRefreshRequest(IRestRequest request, string refreshToken)
		{
			request.AddBody(new RefreshRequest
			{
				ClientId = _clientId,
				ClientSecret = _clientSecret,
				Token = refreshToken
			});
		}

		protected override OAuth2AuthResult HandleAuthResult(AuthenticationResponse response) => new OAuth2AuthResult
		{
			AccessToken = response.AccessToken,
			RefreshToken = response.RefreshToken,
			ExpiresAt = response.IssuedDate.AddSeconds(response.ExpiresIn)
		};
	}
}