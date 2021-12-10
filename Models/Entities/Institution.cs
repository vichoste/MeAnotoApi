using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Institution
/// </summary>
public class Institution : Entity {
	/// <summary>
	/// Events
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<Event> Events { get; set; }
}
