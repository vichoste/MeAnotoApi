using System.Text.Json.Serialization;

using MeAnotoApi.Authentication;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Identity;

namespace MeAnotoApi.Models.Users {
	public class ApplicationUser : IdentityUser {
		[JsonPropertyName(JsonPropertyNames.Run)]
		public string Run { get; set; }
		[JsonPropertyName(JsonPropertyNames.FirstName)]
		public string FirstName { get; set; }
		[JsonPropertyName(JsonPropertyNames.LastName)]
		public string LastName { get; set; }
		[JsonIgnore]
		public virtual Institution Institution { get; set; }
	}
}
