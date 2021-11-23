using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Entities;

namespace MeAnotoApi.Models.Users;
/// <summary>
/// Attendee
/// </summary>
public class Attendee : ApplicationUser {
	/// <summary>
	/// Event instances
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<EventInstance> EventInstances { get; set; }
	/// <summary>
	/// Course instances
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<CourseInstance> CourseInstances { get; set; }
}
