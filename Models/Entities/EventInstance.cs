using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;
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
	public Event Event { get; set; }
	///// <summary>
	///// Course instance
	///// </summary>
	//[JsonIgnore]
	//public CourseInstance CourseInstance { get; set; }
	///// <summary>
	///// Room
	///// </summary>
	//[JsonIgnore]
	//public Room Room { get; set; }
	/// <summary>
	/// Attendees
	/// </summary>
	[JsonIgnore]
	public ICollection<Attendee> Attendees { get; set; }
	/// <summary>
	/// Schedule date
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.DateTime)]
	public DateTime Schedule { get; set; }
	/// <summary>
	/// Creation date
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Creation)]
	public DateTime Creation { get; set; }
	/// <summary>
	/// Cancellation date
	/// </summary>
	[JsonPropertyName(JsonPropertyNames.Cancellation)]
	public DateTime Cancellation { get; set; }
}
