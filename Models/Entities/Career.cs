using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Career
/// </summary>
public class Career : Entity {
	/// <summary>
	/// Creates a career
	/// </summary>
	public Career() => this.Courses = new HashSet<Course>();
	/// <summary>
	/// Campuses
	/// </summary>
	[JsonIgnore]
	public CampusSingular CampusSingular { get; set; }
	/// <summary>
	/// Courses
	/// </summary>
	[JsonIgnore]
	public ICollection<Course> Courses { get; set; }
}
