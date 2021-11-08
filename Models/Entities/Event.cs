using System.Collections.Generic;
using System.Text.Json.Serialization;

using MeAnotoApi.Models.Users;

namespace MeAnotoApi.Models.Entities {
	public class Event : Entity {
		[JsonIgnore]
		public virtual Professor Professor { get; set; }
		[JsonIgnore]
		public virtual Institution Institution { get; set; }
		[JsonIgnore]
		public virtual ICollection<EventInstance> EventInstances { get; set; }
	}
}
