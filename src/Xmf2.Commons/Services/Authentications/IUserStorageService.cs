using System.Threading;
using System.Threading.Tasks;
using Xmf2.Commons.Services.Authentications.Models;

namespace Xmf2.Commons.Services.Authentications
{
	public interface IUserStorageService
	{
		Task Store(AuthenticationDetailStorageModel detail);

		Task Store(AuthenticationDetailStorageModel detail, CancellationToken ct);

		Task<bool> Has();

		Task<bool> Has(CancellationToken ct);

		Task<AuthenticationDetailStorageModel> Get();

		Task<AuthenticationDetailStorageModel> Get(CancellationToken ct);

		Task Delete();

		Task Delete(CancellationToken ct);
	}

}
