using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Information;
using MeAnotoApi.Models.Users;

namespace MeAnotoApi.Models.Entities;
/// <summary>
/// Event
/// </summary>
public class Event : Entity {
	/// <summary>
	/// Creates a event
	/// </summary>
	public Event() => this.Attendees = new HashSet<Attendee>();
	/// <summary>
	/// Event capacity
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Capacity)]
	public virtual int Capacity { get; set; }
	/// <summary>
	/// Creation date
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Start)]
	public virtual DateTime Start { get; set; }
	/// <summary>
	/// Cancellation date
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Ending)]
	public virtual DateTime Cancellation { get; set; }
	/// <summary>
	/// Professor
	/// </summary>
	[JsonIgnore]
	public virtual Professor Professor { get; set; }
	/// <summary>
	/// Institution
	/// </summary>
	[JsonIgnore]
	public virtual Institution Institution { get; set; }
	/// <summary>
	/// Attendees
	/// </summary>
	[JsonIgnore]
	public virtual ICollection<Attendee> Attendees { get; set; }
}
