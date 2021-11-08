using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;

namespace MeAnotoApi.Models.Entities {
	public abstract class Entity {
		[JsonIgnore]
		public int Id { get; set; }
		[JsonPropertyName(JsonPropertyNames.Name)]
		public string Name { get; set; }
	}
}
