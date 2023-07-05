using System.Net;
using Newtonsoft.Json;
using RestSharp.Portable;
using Xmf2.Core.Authentications;

namespace Xmf2.Authentications.OAuth2
{
	public abstract class OAuth2ConfigurationBase
	{
		public Method LoginMethod { get; protected set; } = Method.POST;

		public string LoginUrl { get; protected set; }

		public Method RefreshMethod { get; protected set;} = Method.POST;

		public string RefreshUrl { get; protected set; }

		public Method LogoutMethod { get; protected set; } = Method.DELETE;

		public string LogoutUrl { get; protected set; }

		public abstract OAuth2AuthResult HandleAuthResult(IRestResponse response);

		public abstract void PopulateLoginRequest(IRestRequest request, string login, string password);

		public abstract void PopulateRefreshRequest(IRestRequest request, string refreshToken);

		public abstract void PopulateLogoutRequest(IRestRequest request);
	}

	public abstract class OAuth2ConfigurationBase<TAuthRequestResponse> : OAuth2ConfigurationBase
	{
		//https://fr.wikipedia.org/wiki/Liste_des_codes_HTTP
		protected const int HTTP_STATUS_CODE_RETRY_WITH = 449;
		protected const int HTTP_STATUS_CODE_UPGRADE_REQUIRED = 426;

		

		public override OAuth2AuthResult HandleAuthResult(IRestResponse response)
		{
			if (response.IsSuccess)
			{
				string content = response.Content;
				TAuthRequestResponse responseResult = JsonConvert.DeserializeObject<TAuthRequestResponse>(content);

				OAuth2AuthResult result = HandleAuthResult(responseResult);
				return result;
			}
			switch (response.StatusCode)
			{
				case HttpStatusCode.Unauthorized:
				case HttpStatusCode.Forbidden:
					return new OAuth2AuthResult
					{
						IsSuccess = false,
						ErrorReason = AuthErrorReason.InvalidCredentials,
						ErrorMessage = response.Content,
					};
				case HttpStatusCode.BadRequest:
					return new OAuth2AuthResult
					{
						IsSuccess = false,
						ErrorReason = AuthErrorReason.BadRequest,
						ErrorMessage = response.Content,
					};
				case (HttpStatusCode)HTTP_STATUS_CODE_RETRY_WITH:
				case (HttpStatusCode)HTTP_STATUS_CODE_UPGRADE_REQUIRED:
					return new OAuth2AuthResult
					{
						IsSuccess = false,
						ErrorReason = AuthErrorReason.InvalidAppVersion,
						ErrorMessage = response.Content,
					};
				default:
					return new OAuth2AuthResult
					{
						IsSuccess = false,
						ErrorReason = AuthErrorReason.ServerError,
						ErrorMessage = response.Content,
					};
			}
		}

		protected abstract OAuth2AuthResult HandleAuthResult(TAuthRequestResponse response);
	}
}