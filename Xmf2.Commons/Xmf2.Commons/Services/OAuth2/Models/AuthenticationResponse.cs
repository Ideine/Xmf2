using System;
using Newtonsoft.Json;

namespace Xmf2.Commons.Services.OAuth2.Models
{
	public class AuthenticationResponse
	{
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }

		[JsonProperty("access_token")]
		public string AccessToken { get; set; }

		[JsonProperty("issued_date")]
		public DateTime IssuedDate { get; set; }

		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }
	}
}
