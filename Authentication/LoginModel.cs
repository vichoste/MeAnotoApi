using System.Text.Json.Serialization;

namespace MeAnotoApi.Authentication;

public class LoginModel {
	[JsonPropertyName(JsonPropertyNames.Email)]
	public string Email { get; set; }
	[JsonPropertyName(JsonPropertyNames.Password)]
	public string Password { get; set; }
}
