using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;

namespace Xmf2.Http.RestSharp.Services.DataAccess
{
	public interface IRequestService
	{
		Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true);

		Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true);
	}
}