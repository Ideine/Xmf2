using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Xmf2.Core.Errors;

namespace Xmf2.Core.Services
{
	public class AuthenticationConstants
	{
		public const string NO_AUTH_HEADER = "X-No-Auth";
	}

	public interface IRequestService
	{
		Task<RestResponse> Execute(RestRequest request, CancellationToken ct, bool withAuthentication = true);

		Task<RestResponse<T>> Execute<T>(RestRequest request, CancellationToken ct, bool withAuthentication = true);
	}

	public class RequestService : IRequestService
	{
		private readonly RestClient _client;
		protected IHttpErrorInterpreter ErrorManager { get; }

		public RequestService(RestClient client, IHttpErrorInterpreter errorManager)
		{
			_client = client;
			ErrorManager = errorManager;
		}

		public virtual async Task<RestResponse> Execute(RestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			if (!withAuthentication)
			{
				request.AddHeader(AuthenticationConstants.NO_AUTH_HEADER, true);
			}

			try
			{
				return await _client.ExecuteAsync(request, ct);
			}
			catch (Exception e)
			{
				throw ErrorManager.InterpretException(e);
			}
		}

		public virtual async Task<RestResponse<T>> Execute<T>(RestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			if (!withAuthentication)
			{
				request.AddHeader(AuthenticationConstants.NO_AUTH_HEADER, true);
			}

			try
			{
				return await _client.ExecuteAsync<T>(request, ct);
			}
			catch (Exception e)
			{
				throw ErrorManager.InterpretException(e);
			}
		}
	}
}