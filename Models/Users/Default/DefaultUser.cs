using System.Text.Json.Serialization;

using MeAnotoApi.Models.Entities;

namespace MeAnotoApi.Models.Users.Default {
	public abstract class DefaultUser : User {
		public string Run { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public virtual int InstitutionId { get; set; }
		[JsonIgnore]
		public virtual Institution Institution { get; set; }
	}
}
