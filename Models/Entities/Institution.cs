using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Institution
/// </summary>
public class Institution : Entity {
	///// <summary>
	///// Campuses
	///// </summary>
	//[JsonIgnore]
	//public ICollection<CampusSingular> CampusSingulars { get; set; }
	/// <summary>
	/// Events
	/// </summary>
	[JsonIgnore]
	public ICollection<Event> Events { get; set; }
}
