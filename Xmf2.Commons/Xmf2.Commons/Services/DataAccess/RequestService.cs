using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Commons.ErrorManagers;

namespace Xmf2.Commons.Services
{
	public interface IRequestService
	{
		Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true);

		Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true);
	}

	public class RequestService : IRequestService
	{
		private readonly IRestClient _client;
		protected IHttpErrorManager ErrorManager { get; }

		public RequestService(IRestClient client, IHttpErrorManager errorManager)
		{
			_client = client;
			ErrorManager = errorManager;
		}

		public virtual Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			return ErrorManager.ExecuteAsync(() => _client.Execute(request, ct));
		}

		public virtual Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			return ErrorManager.ExecuteAsync(() => _client.Execute<T>(request, ct));
		}
	}
}
