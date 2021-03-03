using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Core.Authentications;

namespace Xmf2.Authentications.OAuth2
{
	internal class OAuth2Authenticator : TokenAuthenticator
	{
		private OAuth2AuthResult _access;
		protected override string TokenType { get; } = "Bearer";

		public OAuth2AuthResult Access
		{
			get => _access;
			set
			{
				if (_access == value)
				{
					return;
				}

				_access = value;
				if (_access != null)
				{
					SetToken(new TokenAuthentication(_access.AccessToken, _access.ExpiresAt));
				}
				else
				{
					SetToken(null);
				}
			}
		}

		public override bool CanPreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
		{
			return base.CanPreAuthenticate(client, request, credentials) && client is IOAuth2Client;
		}

		protected override async Task Refresh(IRestClient client, IRestRequest request)
		{
			if (!(client is IOAuth2Client oauth2Client))
			{
				throw new NotSupportedException("This authenticator can only be used with an IOAuth2Client");
			}

			if (IsExpired())
			{
				OAuth2AuthResult result = await oauth2Client.Refresh();
				if (result.IsSuccess)
				{
					Access = result;
				}
			}
		}
	}
}