using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Entities;

namespace MeAnotoApi.Models.Users {
	public class Attendee : ApplicationUser {
		[JsonIgnore]
		public virtual ICollection<EventInstance> EventInstances { get; set; }
		[JsonIgnore]
		public virtual ICollection<CourseInstance> CourseInstances { get; set; }
	}
}
