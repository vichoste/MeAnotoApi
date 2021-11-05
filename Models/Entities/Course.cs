using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities {
	public class Course : Entity {
		[JsonIgnore]
		public virtual Career Career { get; set; }
	}
}
