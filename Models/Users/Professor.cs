using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Entities;

namespace MeAnotoApi.Models.Users;
/// <summary>
/// Professor
/// </summary>
public class Professor : ApplicationUser {
	/// <summary>
	/// Events
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<Event> Events { get; set; }
	/// <summary>
	/// Course instances
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<CourseInstance> CourseInstances { get; set; }
}
