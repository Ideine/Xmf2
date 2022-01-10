using Newtonsoft.Json;

namespace Xmf2.Commons.Services.OAuth2.Models
{
	internal class RefreshRequest
	{
		[JsonProperty("token")]
		public string Token { get; set; }

		[JsonProperty("client_id")]
		public string ClientId { get; set; }

		[JsonProperty("client_secret")]
		public string ClientSecret { get; set; }
	}
}
