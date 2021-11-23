using System.Text.Json.Serialization;

namespace MeAnotoApi.Authentication;
/// <summary>
/// Model for login form
/// </summary>
public class LoginModel {
	/// <summary>
	/// Form email
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Email)]
	public string Email { get; set; }
	/// <summary>
	/// Form password
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Password)]
	public string Password { get; set; }
}
