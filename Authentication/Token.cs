using System;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Authentication;

public class Token {
	[JsonPropertyName(JsonPropertyNames.Status)]
	public string Status { get; set; }
	[JsonPropertyName(JsonPropertyNames.Info)]
	public string Info { get; set; }
	[JsonPropertyName(JsonPropertyNames.Expiration)]
	public DateTime Expiration { get; set; }
}
