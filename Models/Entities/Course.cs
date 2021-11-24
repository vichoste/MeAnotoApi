using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Course
/// </summary>
public class Course : Entity {
	/// <summary>
	/// Career
	/// </summary>
	[JsonIgnore]
	public Career Career { get; set; }
}
