using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;
using MeAnotoApi.Models.Users;

namespace MeAnotoApi.Models.Entities {
	public class CourseInstance : Entity {
		[JsonIgnore]
		public virtual Course Course { get; set; }
		[JsonPropertyName(JsonPropertyNames.Year)]
		public string Year { get; set; }
		[JsonPropertyName(JsonPropertyNames.Semester)]
		public string Semester { get; set; }
		[JsonPropertyName(JsonPropertyNames.Section)]
		public string Section { get; set; }
		[JsonIgnore]
		public virtual ICollection<EventInstance> EventInstances { get; set; }
		[JsonIgnore]
		public virtual ICollection<Attendee> Attendees { get; set; }
		[JsonIgnore]
		public virtual ICollection<Professor> Professors { get; set; }
	}
}
