namespace MeAnotoApi.Models.Authentication {
	public sealed class DefaultRegisterModel : RegisterModel {
		public string Run { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Role { get; set; }
		public int InstitutionId { get; set; }
	}
}
