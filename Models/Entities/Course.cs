using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities {
	public class Course : Entity {
		public virtual int CareerId { get; set; }
		[JsonIgnore]
		public virtual Career Career { get; set; }
	}
}
