using System;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Core.Errors;

namespace Xmf2.Core.Services
{
	public class AuthenticationConstants
	{
		public const string NO_AUTH_HEADER = "X-No-Auth";
	}

	public interface IRequestService
	{
		Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true);

		Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true);
	}

	public class RequestService : IRequestService
	{
		private readonly IRestClient _client;
		protected IHttpErrorInterpreter ErrorManager { get; }

		public RequestService(IRestClient client, IHttpErrorInterpreter errorManager)
		{
			_client = client;
			ErrorManager = errorManager;
		}

		public virtual async Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			if (!withAuthentication)
			{
				request.AddHeader(AuthenticationConstants.NO_AUTH_HEADER, true);
			}

			try
			{
				return await _client.Execute(request, ct);
			}
			catch (Exception e)
			{
				throw ErrorManager.InterpretException(e);
			}
		}

		public virtual async Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
		{
			if (!withAuthentication)
			{
				request.AddHeader(AuthenticationConstants.NO_AUTH_HEADER, true);
			}

			try
			{
				return await _client.Execute<T>(request, ct);
			}
			catch (Exception e)
			{
				throw ErrorManager.InterpretException(e);
			}
		}
	}
}