using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Authentication;
/// <summary>
/// Token JSON response when an user logs in successfully
/// </summary>
public class Token {
	/// <summary>
	/// Response status
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Status)]
	public string Status { get; set; }
	/// <summary>
	/// Token response
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Info)]
	public string Info { get; set; }
	/// <summary>
	/// Token expiration response
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Expiration)]
	public DateTime Expiration { get; set; }
	/// <summary>
	/// User roles associated with the logged in user
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Roles)]
	public IList<string> Roles { get; set; }
}
