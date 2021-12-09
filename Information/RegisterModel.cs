using System.Text.Json.Serialization;

namespace MeAnotoApi.Information;
/// <summary>
/// Model for registration form
/// </summary>
public sealed class RegisterModel {
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
	/// <summary>
	/// Form Chilean RUN
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Run)]
	public string Run { get; set; }
	/// <summary>
	/// Form first name
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.FirstName)]
	public string FirstName { get; set; }
	/// <summary>
	/// Form last name
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.LastName)]
	public string LastName { get; set; }
}
