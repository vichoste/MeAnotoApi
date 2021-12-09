using System.Text.Json.Serialization;

using MeAnotoApi.Information;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Abstract entity
/// </summary>
public abstract class Entity {
	/// <summary>
	/// ID
	/// </summary>
	[JsonIgnore]
	public int Id { get; set; }
	/// <summary>
	/// Name
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Name)]
	public string Name { get; set; }
}
