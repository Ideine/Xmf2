using System;

namespace Xmf2.Commons.Services.Authentications.Models
{
	public class AuthenticationDetailStorageModel
	{
		public string RefreshToken { get; set; }

		public string AccessToken { get; set; }

		public DateTime ExpireDate { get; set; }
	}
}
