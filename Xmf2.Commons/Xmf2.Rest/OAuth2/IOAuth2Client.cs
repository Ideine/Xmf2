using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;

namespace Xmf2.Rest.OAuth2
{
	public interface IOAuth2Client : IRestClient
	{
		OAuth2ConfigurationBase Configuration { get; set; }

		string RefreshToken { get; }

		string AccessToken { get; }

		event EventHandler<OAuth2AuthResult> OnAuthSuccess;

		event EventHandler<OAuth2AuthResult> OnAuthError;

		Task<OAuth2AuthResult> Login(string login, string password);

		Task<OAuth2AuthResult> Refresh();

		Task<OAuth2AuthResult> Refresh(string refreshToken);

		Task<OAuth2AuthResult> Login(string login, string password, CancellationToken ct);

		Task<OAuth2AuthResult> Refresh(string refreshToken, CancellationToken ct);

		Task<OAuth2AuthResult> Refresh(CancellationToken ct);

		void Logout();
	}
}