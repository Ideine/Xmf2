using System.Threading.Tasks;

namespace Xmf2.Authentications.OAuth2.Authentication
{
	public interface IAuthenticationService
	{
		bool CanLoginWithRefreshToken();

		Task<bool> LoginWithCredentials(string login, string password);

		Task<bool> LoginWithRefreshToken();

		Task Logout();
	}
}