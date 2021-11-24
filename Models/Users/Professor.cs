using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Entities;

namespace MeAnotoApi.Models.Users;
/// <summary>
/// Professor
/// </summary>
public class Professor : ApplicationUser {
	/// <summary>
	/// Creates a professor
	/// </summary>
	public Professor() {
		this.Events = new HashSet<Event>();
		this.CourseInstances = new HashSet<CourseInstance>();
	}
	/// <summary>
	/// Events
	/// </summary>
	[JsonIgnore]
	public ICollection<Event> Events { get; set; }
	/// <summary>
	/// Course instances
	/// </summary>
	[JsonIgnore]
	public ICollection<CourseInstance> CourseInstances { get; set; }
}
