using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Room
/// </summary>
public class Room : Entity {
	/// <summary>
	/// Capacity
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Capacity)]
	public int Capacity { get; set; }
	/// <summary>
	/// Campus
	/// </summary>
	[JsonIgnore]
	public virtual CampusSingular CampusSingular { get; set; }
	/// <summary>
	/// Event instances
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<EventInstance> EventInstances { get; set; }
}
