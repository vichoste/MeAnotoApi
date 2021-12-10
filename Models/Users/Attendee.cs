using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Entities;

namespace MeAnotoApi.Models.Users;
/// <summary>
/// Attendee
/// </summary>
public class Attendee : ApplicationUser {
	/// <summary>
	/// Creates an attendee
	/// </summary>
	public Attendee() => this.EventInstances = new HashSet<EventInstance>();
	/// <summary>
	/// Event instances
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<EventInstance> EventInstances { get; set; }
}
