using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Commons.Services.Authentications
{
	public interface IAuthenticationService
	{
		IObservable<bool> IsLogged { get; }

		IObservable<bool> LoginWithCredentials(string login, string password);

		IObservable<bool> LoginWithCredentials(string login, string password, CancellationToken ct);

		IObservable<bool> LoginWithRefreshToken();

		IObservable<bool> LoginWithRefreshToken(CancellationToken ct);

		Task<bool> CanLoginWithRefreshToken();

		Task<bool> CanLoginWithRefreshToken(CancellationToken ct);

		Task Logout();

		Task Logout(CancellationToken ct);
	}

}
