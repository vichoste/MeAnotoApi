using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Campus
/// </summary>
public class CampusSingular : Entity {
	/// <summary>
	/// Creates a campus
	/// </summary>
	public CampusSingular() {
		this.Careers = new HashSet<Career>();
		this.Rooms = new HashSet<Room>();
	}
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
