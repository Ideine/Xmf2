using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Xmf2.Commons.Errors;
using Xmf2.Commons.Exceptions;
using Xmf2.Commons.Helpers;
using Xmf2.Commons.Logs;
using Xmf2.Commons.OAuth2;
using Xmf2.Commons.Services.Authentications;
using Xmf2.Commons.Services.Authentications.Models;
using Xmf2.Rest.Caches;

namespace Xmf2.Rx.Services.Authentications
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IOAuth2Client _client;
		private readonly IUserStorageService _storageService;
		private readonly ILogger _logger;
		private readonly IHttpErrorHandler _errorManager;
		private readonly Subject<bool> _isLogged = new Subject<bool>();

		public IObservable<bool> IsLogged { get; }

		public AuthenticationService(IOAuth2Client client, IUserStorageService storageService, ILogger logger, IHttpErrorHandler errorManager)
		{
			_client = client;
			_storageService = storageService;
			_logger = logger;
			_errorManager = errorManager;
			IsLogged = _isLogged.StartWith(false).ToObservableForBinding();

			_client.OnAuthSuccess += OnClientAuthenticationSuccess;
			_client.OnAuthError += OnClientAuthenticationError;
		}

		protected virtual void OnClientAuthenticationSuccess(object sender, OAuth2AuthResult result)
		{
			_isLogged.OnNext(true);
			_storageService.Store(new AuthenticationDetailStorageModel
			{
				AccessToken = result.AccessToken,
				RefreshToken = result.RefreshToken,
				ExpireDate = result.ExpiresAt
			});
		}

		protected virtual void OnClientAuthenticationError(object sender, OAuth2AuthResult e)
		{
			_logger.LogWarning(message: $"{nameof(AuthenticationService)}/Unable to authenticate {e.ErrorReason} : {e.ErrorMessage}");

			if (e.ErrorReason == AuthErrorReason.InvalidAppVersion)
			{
				throw new InvalidAppVersionException();
			}
		}

		public IObservable<bool> LoginWithCredentials(string login, string password)
		{
			return LoginWithCredentials(login, password, CancellationToken.None);
		}

		public IObservable<bool> LoginWithCredentials(string login, string password, CancellationToken ct)
		{
			return _errorManager.ExecuteAsync(() => _client.Login(login, password, ct))
								.Select(loginResult =>
			{
				CacheEngine.InvalidateScope(CacheEngine.SCOPE_USER);

				if (loginResult.IsSuccess)
				{
					OnLogged();
				}

				return loginResult.IsSuccess;
			});
		}

		public IObservable<bool> LoginWithRefreshToken()
		{
			return LoginWithRefreshToken(CancellationToken.None);
		}

		public IObservable<bool> LoginWithRefreshToken(CancellationToken ct)
		{
			return Observable.FromAsync(async token =>
			{
				if (!await _storageService.Has(ct))
				{
					return false;
				}

				AuthenticationDetailStorageModel authDetail = await _storageService.Get(ct);

				OAuth2AuthResult result = await _errorManager.ExecuteAsync(() => _client.Refresh(authDetail.RefreshToken, ct));

				if (result.IsSuccess)
				{
					OnLogged();
				}

				return result.IsSuccess;
			});
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
			_isLogged.OnNext(false);
			_storageService.Delete(ct);
			_client.Logout();
			CacheEngine.InvalidateScope(CacheEngine.SCOPE_USER);
			OnLogout();
			return TaskHelper.CompletedTask;
		}

		protected virtual void OnLogged()
		{

		}

		protected virtual void OnLogout()
		{

		}
	}
}
