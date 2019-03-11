using System;

namespace Xmf2.Authentications.OAuth2.Authentication
{
	public class AuthenticationDetailStorageModel
	{
		public string RefreshToken { get; set; }

		public string AccessToken { get; set; }

		public DateTime ExpireDate { get; set; }
	}
}