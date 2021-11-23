using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Campus
/// </summary>
public class CampusSingular : Entity {
	/// <summary>
	/// Institution
	/// </summary>
	[JsonIgnore]
	public virtual Institution Institution { get; set; }
	/// <summary>
	/// Careers
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<Career> Careers { get; set; }
	/// <summary>
	/// Rooms
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<Room> Rooms { get; set; }
}
