using System.Threading;
using System.Threading.Tasks;
using Xmf2.Commons.Services.Authentications.Models;

namespace Xmf2.Commons.Services.Authentications
{

	public class InMemoryUserStorageService : IUserStorageService
	{
		private AuthenticationDetailStorageModel _detail;

		public Task Store(AuthenticationDetailStorageModel detail)
		{
			return Store(detail, CancellationToken.None);
		}

		public Task Store(AuthenticationDetailStorageModel detail, CancellationToken ct)
		{
			_detail = detail;
			return Task.CompletedTask;
		}

		public Task<bool> Has()
		{
			return Has(CancellationToken.None);
		}

		public Task<bool> Has(CancellationToken ct)
		{
			return Task.FromResult(_detail != null);
		}

		public Task<AuthenticationDetailStorageModel> Get()
		{
			return Get(CancellationToken.None);
		}

		public Task<AuthenticationDetailStorageModel> Get(CancellationToken ct)
		{
			return Task.FromResult(_detail);
		}

		public Task Delete()
		{
			return Delete(CancellationToken.None);
		}

		public Task Delete(CancellationToken ct)
		{
			_detail = null;
			return Task.CompletedTask;
		}
	}

}
