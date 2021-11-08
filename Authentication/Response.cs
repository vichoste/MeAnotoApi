using System.Text.Json.Serialization;

namespace MeAnotoApi.Authentication {
	public sealed class Response {
		[JsonPropertyName(JsonPropertyNames.Status)]
		public string Status { get; set; }
		[JsonPropertyName(JsonPropertyNames.Message)]
		public string Message { get; set; }
	}
}
