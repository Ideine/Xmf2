using System;
using Xmf2.Core.Authentications;

namespace Xmf2.Authentications.OAuth2
{
	public class OAuth2AuthResult
	{
		public bool IsSuccess { get; set; }

		public AuthErrorReason ErrorReason { get; set; }

		public string ErrorMessage { get; set; }

		public string RefreshToken { get; set; }

		public string AccessToken { get; set; }

		//VJU 31/10/2023 : be careful, there is an issue if token is store 1 day before timezone switch where token is store with an invalid expiration date (in october)
		public DateTime ExpiresAt { get; set; }
	}
}
