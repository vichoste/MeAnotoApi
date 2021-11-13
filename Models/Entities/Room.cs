using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;

namespace MeAnotoApi.Models.Entities;

public class Room : Entity {
	[JsonPropertyName(JsonPropertyNames.Capacity)]
	public int Capacity { get; set; }
	[JsonIgnore]
	public virtual CampusSingular CampusSingular { get; set; }
	[JsonIgnore]
	public virtual ICollection<EventInstance> EventInstances { get; set; }
}
