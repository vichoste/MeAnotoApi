using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;
using MeAnotoApi.Models.Users;

namespace MeAnotoApi.Models.Entities;

public class EventInstance : Entity {
	[JsonIgnore]
	public virtual Event Event { get; set; }
	[JsonIgnore]
	public virtual CourseInstance CourseInstance { get; set; }
	[JsonIgnore]
	public virtual Room Room { get; set; }
	[JsonIgnore]
	public virtual ICollection<Attendee> Attendees { get; set; }
	[JsonPropertyName(JsonPropertyNames.DateTime)]
	public DateTime DateTime { get; set; }
	[JsonPropertyName(JsonPropertyNames.Creation)]
	public DateTime Creation { get; set; }
	[JsonPropertyName(JsonPropertyNames.Cancellation)]
	public DateTime Cancellation { get; set; }
}
