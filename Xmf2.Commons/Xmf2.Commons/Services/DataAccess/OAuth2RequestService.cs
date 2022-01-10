using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Commons.ErrorManagers;
using Xmf2.Rest.OAuth2;

namespace Xmf2.Commons.Services
{
	public class OAuth2RequestService : RequestService
	{
		private readonly IOAuth2Client _client;

		public OAuth2RequestService(IRestClient client, IOAuth2Client authenticatedClient, IHttpErrorManager errorManager) : base(client, errorManager)
		{
			_client = authenticatedClient;
		}

		public override Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			if (withAuthentication)
			{
				return ErrorManager.ExecuteAsync(() => _client.Execute(request, ct));
			}

			return base.Execute(request, ct, withAuthentication);
		}

		public override Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			if (withAuthentication)
			{
				return ErrorManager.ExecuteAsync(() => _client.Execute<T>(request, ct));
			}

			return base.Execute<T>(request, ct, withAuthentication);
		}
	}
}
