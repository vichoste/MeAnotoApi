using System.Text.Json.Serialization;

using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Identity;

namespace MeAnotoApi.Models.Users {
	public class ApplicationUser : IdentityUser {
		public string Run { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[JsonIgnore]
		public virtual Institution Institution { get; set; }
	}
}
