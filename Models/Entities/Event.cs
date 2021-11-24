using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Users;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Event
/// </summary>
public class Event : Entity {
	/// <summary>
	/// Creates a event
	/// </summary>
	public Event() => this.EventInstances = new HashSet<EventInstance>();
	/// <summary>
	/// Professor
	/// </summary>
	[JsonIgnore]
	public virtual Professor Professor { get; set; }
	/// <summary>
	/// Institution
	/// </summary>
	[JsonIgnore]
	public virtual Institution Institution { get; set; }
	/// <summary>
	/// Event instances
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<EventInstance> EventInstances { get; set; }
}
