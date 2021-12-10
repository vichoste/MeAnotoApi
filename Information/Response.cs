using System.Text.Json.Serialization;

using MeAnotoApi.Strings;

namespace MeAnotoApi.Information;
/// <summary>
/// HTML response JSON
/// </summary>
public sealed class Response {
	/// <summary>
	/// Response status
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Status)]
	public string Status { get; set; }
	/// <summary>
	/// Message status
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Message)]
	public string Message { get; set; }
	/// <summary>
	/// Entity result
	/// </summary>
	[JsonPropertyName(Entities.Entity)]
	public EntityResponse EntityResponse { get; set; }
}
