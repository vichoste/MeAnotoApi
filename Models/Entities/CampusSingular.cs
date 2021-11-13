using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities;

public class CampusSingular : Entity {
	[JsonIgnore]
	public virtual Institution Institution { get; set; }
	[JsonIgnore]
	public virtual ICollection<Career> Careers { get; set; }
	[JsonIgnore]
	public virtual ICollection<Room> Rooms { get; set; }
}
