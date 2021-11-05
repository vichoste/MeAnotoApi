using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities {
	public class Career : Entity {
		[JsonIgnore]
		public virtual CampusSingular CampusSingular { get; set; }
		[JsonIgnore]
		public virtual ICollection<Course> Courses { get; set; }
	}
}