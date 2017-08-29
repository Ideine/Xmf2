using System;
using System.Net;
using Newtonsoft.Json;
using RestSharp.Portable;

namespace Xmf2.Rest.OAuth2
{
	public abstract class OAuth2ConfigurationBase
	{
		public Method LoginMethod { get; protected set; } = Method.POST;

		public string LoginUrl { get; protected set; }

		public Method RefreshMethod { get; protected set;} = Method.POST;

		public string RefreshUrl { get; protected set; }

		public TimeSpan TokenSafetyMargin { get; protected set; } = TimeSpan.FromSeconds(30);

		public abstract OAuth2AuthResult HandleAuthResult(IRestResponse response);

		public abstract void PopulateLoginRequest(IRestRequest request, string login, string password);

		public abstract void PopulateRefreshRequest(IRestRequest request, string refreshToken);
	}

	public abstract class OAuth2ConfigurationBase<TAuthRequestResponse> : OAuth2ConfigurationBase
	{
		public override OAuth2AuthResult HandleAuthResult(IRestResponse response)
		{
			if (response.IsSuccess)
			{
				string content = response.Content;
				TAuthRequestResponse responseResult = JsonConvert.DeserializeObject<TAuthRequestResponse>(content);

				OAuth2AuthResult result = HandleAuthResult(responseResult);
				result.IsSuccess = true;
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
				case (HttpStatusCode)449:
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