using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Entities;

namespace MeAnotoApi.Models.Users {
	public class Professor : ApplicationUser {
		[JsonIgnore]
		public virtual ICollection<Event> Events { get; set; }
		[JsonIgnore]
		public virtual ICollection<CourseInstance> CourseInstances { get; set; }
	}
}
