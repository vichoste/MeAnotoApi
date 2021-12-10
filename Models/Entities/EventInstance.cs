using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Information;
using MeAnotoApi.Models.Users;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Event instance
/// </summary>
public class EventInstance : Entity {
	/// <summary>
	/// Creates a event instance
	/// </summary>
	public EventInstance() => this.Attendees = new HashSet<Attendee>();
	/// <summary>
	/// Event
	/// </summary>
	[JsonIgnore]
	public virtual Event Event { get; set; }
	/// <summary>
	/// Attendees
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<Attendee> Attendees { get; set; }
	/// <summary>
	/// Schedule date
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.DateTime)]
	public virtual DateTime Schedule { get; set; }
	/// <summary>
	/// Creation date
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Creation)]
	public virtual DateTime Creation { get; set; }
	/// <summary>
	/// Cancellation date
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Cancellation)]
	public virtual DateTime Cancellation { get; set; }
}
