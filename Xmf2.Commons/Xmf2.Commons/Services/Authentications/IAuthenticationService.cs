using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Commons.Services.Authentications
{

	public interface IAuthenticationService
	{
		bool IsLogged { get; }

		Task<bool> LoginWithCredentials(string login, string password);

		Task<bool> LoginWithCredentials(string login, string password, CancellationToken ct);

		Task<bool> LoginWithRefreshToken();

		Task<bool> LoginWithRefreshToken(CancellationToken ct);

		Task<bool> CanLoginWithRefreshToken();

		Task<bool> CanLoginWithRefreshToken(CancellationToken ct);

		Task Logout();

		Task Logout(CancellationToken ct);
	}

}
