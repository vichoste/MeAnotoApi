using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Contexts {
	public class MeAnotoContext : IdentityDbContext<User> {
		public MeAnotoContext(DbContextOptions<MeAnotoContext> options) : base(options) {
		}
		protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder);
	}
}
