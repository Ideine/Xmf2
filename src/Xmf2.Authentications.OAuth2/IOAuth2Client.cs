//using System;
//using System.Threading.Tasks;
//using Xmf2.Core.Authentications;

//namespace Xmf2.Authentications.OAuth2
//{
//	public interface IOAuth2Client : IAuthenticatedRestClient
//	{
//		OAuth2ConfigurationBase Configuration { get; set; }

//		event EventHandler<OAuth2AuthResult> OnAuthSuccess;

//		Task<OAuth2AuthResult> Login(string login, string password);

//		Task<OAuth2AuthResult> Refresh();

//		void SetAuthenticationTokens(OAuth2AuthResult token);
//	}
//}