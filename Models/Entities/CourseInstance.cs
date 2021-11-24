using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;
using MeAnotoApi.Models.Users;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Course instance
/// </summary>
public class CourseInstance : Entity {
	/// <summary>
	/// Creates a course instance
	/// </summary>
	public CourseInstance() {
		this.EventInstances = new HashSet<EventInstance>();
		this.Attendees = new HashSet<Attendee>();
		this.Professors = new HashSet<Professor>();
	}
	/// <summary>
	/// Course
	/// </summary>
	[JsonIgnore]
	public virtual Course Course { get; set; }
	/// <summary>
	/// Year
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Year)]
	public string Year { get; set; }
	/// <summary>
	/// Semester
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Semester)]
	public string Semester { get; set; }
	/// <summary>
	/// Section
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Section)]
	public string Section { get; set; }
	/// <summary>
	/// Event instances
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<EventInstance> EventInstances { get; set; }
	/// <summary>
	/// Attendees
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<Attendee> Attendees { get; set; }
	/// <summary>
	/// Professors
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<Professor> Professors { get; set; }
}
