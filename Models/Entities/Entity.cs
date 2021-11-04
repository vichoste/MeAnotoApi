using System.Text.Json.Serialization;

namespace MeAnotoApi.Models.Entities {
	public abstract class Entity {
		[JsonIgnore]
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
