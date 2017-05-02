using System;

namespace Xmf2.Rest.OAuth2
{
	public class OAuth2AuthResult
	{
		public bool IsSuccess { get; set; }

		public AuthErrorReason ErrorReason { get; set; }

		public string ErrorMessage { get; set; }

		public string RefreshToken { get; set; }

		public string AccessToken { get; set; }

		public DateTime ExpiresAt { get; set; }
	}
}