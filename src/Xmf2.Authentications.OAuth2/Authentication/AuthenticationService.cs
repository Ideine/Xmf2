//using System;
//using System.Threading.Tasks;
//using Xmf2.Core.Errors;

//namespace Xmf2.Authentications.OAuth2.Authentication
//{
//	public class AuthenticationService : IAuthenticationService
//	{
//		//private readonly IOAuth2Client _client;
//		//private readonly IUserStorageService _storageService;
//		private readonly IHttpErrorInterpreter _errorInterpreter;
//		private readonly Action<string> _logFunction;

//		//public AuthenticationService(IOAuth2Client client, IUserStorageService storageService, IHttpErrorInterpreter errorInterpreter, Action<string> logFunction)
//		//{
//		//	_client = client;
//		//	_storageService = storageService;
//		//	_errorInterpreter = errorInterpreter;
//		//	_logFunction = logFunction;
//		//	_client.OnAuthSuccess += OnAuthSuccess;
//		//}

//		//private void OnAuthSuccess(object sender, OAuth2AuthResult authResult)
//		//{
//		//	_storageService.Store(new AuthenticationDetailStorageModel
//		//	{
//		//		RefreshToken = authResult.RefreshToken,
//		//		AccessToken = authResult.AccessToken,
//		//		ExpireDate = authResult.ExpiresAt
//		//	});
//		//	_client.SetAuthenticationTokens(authResult);
//		//}

//		//public bool CanLoginWithRefreshToken()
//		//{
//		//	var authToken = _storageService.Get();
//		//	if (authToken != null)
//		//	{
//		//		_client.SetAuthenticationTokens(new OAuth2AuthResult
//		//		{
//		//			IsSuccess = true,
//		//			AccessToken = authToken.AccessToken,
//		//			RefreshToken = authToken.RefreshToken,
//		//			ExpiresAt = authToken.ExpireDate
//		//		});
//		//	}

//		//	return authToken != null;
//		//}

//		public async Task<bool> LoginWithCredentials(string login, string password)
//		{
//			try
//			{
//				var authResult = await _client.Login(login, password);
//				string status = authResult.IsSuccess ? "SUCCESS" : "FAILED";
//				_logFunction?.Invoke($"LOG {status} : {login}");
//				return authResult.IsSuccess;
//			}
//			catch (Exception e)
//			{
//				throw _errorInterpreter.InterpretException(e);
//			}
//		}

//		//public async Task<bool> LoginWithRefreshToken()
//		//{
//		//	try
//		//	{
//		//		if (CanLoginWithRefreshToken())
//		//		{
//		//			var result = await _client.Refresh();

//		//			string status = result.IsSuccess ? "SUCCESS" : "FAILED";
//		//			_logFunction?.Invoke($"REFRESH {status}");

//		//			return result.IsSuccess;
//		//		}

//		//		return false;
//		//	}
//		//	catch (Exception e)
//		//	{
//		//		throw _errorInterpreter.InterpretException(e);
//		//	}
//		//}

//		public async Task Logout()
//		{
//			await _client.Logout();
//			_storageService.Delete();
//		}
//	}
//}