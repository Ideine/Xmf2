using System.Threading;
using System.Threading.Tasks;
using Ideine.Rest.Caches;
using Ideine.Rest.OAuth2;
using Xmf2.Commons.ErrorManagers;
using Xmf2.Commons.Logs;
using Xmf2.Commons.Services.Authentications.Models;

namespace Xmf2.Commons.Services.Authentications
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IOAuth2Client _client;
		private readonly IUserStorageService _storageService;
		private readonly ILogger _logger;
		private readonly IHttpErrorManager _errorManager;

		public bool IsLogged { get; private set; }

		public AuthenticationService(IOAuth2Client client, IUserStorageService storageService, ILogger logger, IHttpErrorManager errorManager)
		{
			_client = client;
			_storageService = storageService;
			_logger = logger;
			_errorManager = errorManager;

			_client.OnAuthSuccess += OnClientAuthenticationSuccess;
			_client.OnAuthError += OnClientAuthenticationError;
		}

		protected virtual async void OnClientAuthenticationSuccess(object sender, OAuth2AuthResult result)
		{
			IsLogged = true;
			await _storageService.Store(new AuthenticationDetailStorageModel
			{
				AccessToken = result.AccessToken,
				RefreshToken = result.RefreshToken,
				ExpireDate = result.ExpiresAt
			});
		}

		protected virtual void OnClientAuthenticationError(object sender, OAuth2AuthResult e)
		{
			_logger.LogWarning(message: $"{nameof(AuthenticationService)}/Unable to authenticate {e.ErrorReason} : {e.ErrorMessage}");
		}

		public Task<bool> LoginWithCredentials(string login, string password)
		{
			return LoginWithCredentials(login, password, CancellationToken.None);
		}

		public async Task<bool> LoginWithCredentials(string login, string password, CancellationToken ct)
		{
			OAuth2AuthResult result = await _errorManager.ExecuteAsync(() => _client.Login(login, password, ct));
			CacheEngine.InvalidateScope(CacheEngine.SCOPE_USER);
			return result.IsSuccess;
		}

		public Task<bool> LoginWithRefreshToken()
		{
			return LoginWithRefreshToken(CancellationToken.None);
		}

		public async Task<bool> LoginWithRefreshToken(CancellationToken ct)
		{
			if (!await _storageService.Has(ct))
			{
				return false;
			}

			AuthenticationDetailStorageModel authDetail = await _storageService.Get(ct);

			OAuth2AuthResult result = await _errorManager.ExecuteAsync(() => _client.Refresh(authDetail.RefreshToken, ct));
			return result.IsSuccess;
		}

		public Task<bool> CanLoginWithRefreshToken()
		{
			return _storageService.Has(CancellationToken.None);
		}

		public Task<bool> CanLoginWithRefreshToken(CancellationToken ct)
		{
			return _storageService.Has(ct);
		}

		public Task Logout()
		{
			return Logout(CancellationToken.None);
		}

		public Task Logout(CancellationToken ct)
		{
			IsLogged = false;
			_storageService.Delete(ct);
			_client.Logout();
			CacheEngine.InvalidateScope(CacheEngine.SCOPE_USER);

			return TaskHelper.CompletedTask;
		}
	}
}
