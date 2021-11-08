using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Users;

namespace MeAnotoApi.Models.Entities {
	public class EventInstance {
		[JsonIgnore]
		public virtual Event Event { get; set; }
		[JsonIgnore]
		public virtual CourseInstance CourseInstance { get; set; }
		[JsonIgnore]
		public virtual Room Room { get; set; }
		[JsonIgnore]
		public virtual ICollection<Attendee> Attendees { get; set; }
	}
}
