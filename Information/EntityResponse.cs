using System.Text.Json.Serialization;

namespace MeAnotoApi.Information;

/// <summary>
/// Object response
/// </summary>
public class EntityResponse {
	/// <summary>
	/// Entity ID 
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Id)]
	public int Id { get; set; }
	/// <summary>
	/// Entity name
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Name)]
	public string Name { get; set; }
	/// <summary>
	/// Entity owner
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Owner)]
	public string Owner { get; set; }
}
