using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities {
	public class Institution : Entity {
		[JsonIgnore]
		public virtual ICollection<CampusSingular> CampusSingulars { get; set; }
		[JsonIgnore]
		public virtual ICollection<Event> Events { get; set; }
	}
}
