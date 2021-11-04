using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities {
	public class Institution : Entity {
		[JsonIgnore]
		public virtual ICollection<CampusSingular> CampusSingulars { get; set; }
	}
}
