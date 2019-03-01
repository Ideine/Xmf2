using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp.Portable;
using Xmf2.Core.Exceptions;
using Xmf2.Core.Services;

namespace Xmf2.Core.Authentications
{
	public abstract class TokenAuthenticator : IAuthenticator
	{
		private static readonly SemaphoreSlim _refreshMutex = new SemaphoreSlim(1, 1);

		protected virtual string AuthorizationHeader { get; } = "Authorization";

		protected abstract string TokenType { get; }

		protected virtual TimeSpan SafetySecondsMargin { get; } = TimeSpan.FromSeconds(15);

		public TokenAuthentication Token { get; set; }

		public virtual bool CanPreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
		{
			return Token != null && !request.Parameters.Any(x => x.Name == AuthenticationConstants.NO_AUTH_HEADER && x.Type == ParameterType.HttpHeader);
		}

		public virtual bool CanPreAuthenticate(IHttpClient client, IHttpRequestMessage request, ICredentials credentials)
		{
			return false;
		}

		public virtual bool CanHandleChallenge(IHttpClient client, IHttpRequestMessage request, ICredentials credentials, IHttpResponseMessage response)
		{
			return Token != null && response.StatusCode == HttpStatusCode.Unauthorized && !request.Headers.Contains(AuthenticationConstants.NO_AUTH_HEADER);
		}

		public virtual async Task PreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
		{
			if (IsExpired())
			{
				await _refreshMutex.WaitAsync();
				try
				{
					await Refresh(client, request);
				}
				finally
				{
					_refreshMutex.Release();
				}
			}

			request.AddHeader(AuthorizationHeader, $"{TokenType} {GetToken()}");
		}

		protected abstract Task Refresh(IRestClient client, IRestRequest request);

		public Task PreAuthenticate(IHttpClient client, IHttpRequestMessage request, ICredentials credentials)
		{
			throw new NotSupportedException();
		}

		public Task HandleChallenge(IHttpClient client, IHttpRequestMessage request, ICredentials credentials, IHttpResponseMessage response)
		{
			if (Token != null && response != null && response.StatusCode == HttpStatusCode.Unauthorized)
			{
				Token.ExpireDate = DateTime.MinValue;
			}

			return Task.CompletedTask; // refresh token will be done on next request call
		}

		protected bool IsExpired()
		{
			DateTime? expireDate = Token?.ExpireDate;
			if (expireDate.HasValue)
			{
				return DateTime.Now.Add(SafetySecondsMargin) > expireDate.Value;
			}

			throw new AccessDataException(AccessDataException.ErrorType.UnAuthorized);
		}

		protected string GetToken()
		{
			string token = Token?.Token;
			if (string.IsNullOrEmpty(token))
			{
				throw new AccessDataException(AccessDataException.ErrorType.UnAuthorized);
			}

			return token;
		}
	}
}