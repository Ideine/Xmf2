using Newtonsoft.Json;

namespace Xmf2.Commons.Services.OAuth2.Models
{
	internal class LoginRequest
	{
		[JsonProperty("login")]
		public string Login { get; set; }

		[JsonProperty("password")]
		public string Password { get; set; }

		[JsonProperty("client_id")]
		public string ClientId { get; set; }

		[JsonProperty("client_secret")]
		public string ClientSecret { get; set; }
	}
}
