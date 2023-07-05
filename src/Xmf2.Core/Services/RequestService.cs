using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Core.Authentications;
using Xmf2.Core.Errors;
using Xmf2.Core.Exceptions;

namespace Xmf2.Core.Services
{
	/// <summary>
	/// Execute les RestRequest avec ou sans authentification, s'assure d'obtenir une <see cref="IRestResponse"/>.
	/// Normalise les exception gérées par le <see cref="IHttpErrorInterpreter"/> en <see cref="AccessDataException"/>
	/// </summary>
	public interface IRequestService
	{
		/// <exception cref="AccessDataException"></exception>
		Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true);

		/// <exception cref="AccessDataException"></exception>
		Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true);
	}

	public class RequestService : IRequestService
	{
		private readonly IRestClient _authRestClient;
		private readonly INoAuthRestClient _noAuthRestClient;

		private IHttpErrorInterpreter _httpErrorInterpreter { get; }

		public RequestService(IRestClient authRestClient, INoAuthRestClient noAuthRestClient, IHttpErrorInterpreter httpErrorInterpreter)
		{
			_authRestClient   = authRestClient;
			_noAuthRestClient = noAuthRestClient;
			_httpErrorInterpreter = httpErrorInterpreter;
		}

		public virtual Task<IRestResponse> Execute(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
			=> InternalExecute<IRestResponse>(request, ct, withAuthentication, GetClient(withAuthentication).Execute);

		public virtual Task<IRestResponse<T>> Execute<T>(IRestRequest request, CancellationToken ct, bool withAuthentication = true)
			=> InternalExecute<IRestResponse<T>>(request, ct, withAuthentication, GetClient(withAuthentication).Execute<T>);

		private async Task<TRestResponse> InternalExecute<TRestResponse>(IRestRequest request, CancellationToken ct, bool withAuthentication,
			Func<IRestRequest, CancellationToken, Task<TRestResponse>> executor)
			where TRestResponse : IRestResponse
		{
			try
			{
				var response = await executor(request, ct);
				return response;
			}
			catch (Exception e) when (_httpErrorInterpreter.TryInterpretException(e, out var iEx))
			{
				throw iEx;
			}
		}

		private IRestClient GetClient(bool withAuth) => withAuth ? _authRestClient : _noAuthRestClient;
	}
}