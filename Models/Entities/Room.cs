using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Room
/// </summary>
public class Room : Entity {
	/// <summary>
	/// Creates a room
	/// </summary>
	public Room() => this.EventInstances = new HashSet<EventInstance>();
	/// <summary>
	/// Capacity
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Capacity)]
	public int Capacity { get; set; }
	/// <summary>
	/// Campus
	/// </summary>
	[JsonIgnore]
	public CampusSingular CampusSingular { get; set; }
	/// <summary>
	/// Event instances
	/// </summary>
	[JsonIgnore]
	public ICollection<EventInstance> EventInstances { get; set; }
}
