using System.Text.Json.Serialization;

namespace MeAnotoApi.Authentication {
	public sealed class RegisterModel {
		[JsonPropertyName(JsonPropertyNames.Email)]
		public string Email { get; set; }
		[JsonPropertyName(JsonPropertyNames.Password)]
		public string Password { get; set; }
		[JsonPropertyName(JsonPropertyNames.Run)]
		public string Run { get; set; }
		[JsonPropertyName(JsonPropertyNames.FirstName)]
		public string FirstName { get; set; }
		[JsonPropertyName(JsonPropertyNames.LastName)]
		public string LastName { get; set; }
	}
}
