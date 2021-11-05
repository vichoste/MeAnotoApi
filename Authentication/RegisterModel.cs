namespace MeAnotoApi.Authentication {
	public sealed class RegisterModel {
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Run { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Role { get; set; }
	}
}
