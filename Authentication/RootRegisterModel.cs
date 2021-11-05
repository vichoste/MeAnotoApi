namespace MeAnotoApi.Authentication {
	public sealed class RootRegisterModel : RegisterModel {
		public string UserName { get; set; }
		public string Role { get; set; }
	}
}
