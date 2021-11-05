using MeAnotoApi.Models.Entities;
using MeAnotoApi.Models.Users;
using MeAnotoApi.Models.Users.Default;
using MeAnotoApi.Models.Users.Root;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Contexts {
	public class MeAnotoContext : IdentityDbContext<User> {
		public DbSet<AttendeeUser> AttendeeUsers { get; set; }
		public DbSet<ProfessorUser> ProfessorUsers { get; set; }
		public DbSet<AdministratorUser> AdministratorUsers { get; set; }
		public DbSet<ManagerUser> ManagerUsers { get; set; }
		public DbSet<Institution> Institutions { get; set; }
		public DbSet<CampusSingular> CampusSingulars { get; set; }
		public DbSet<Career> Careers { get; set; }
		public DbSet<Course> Courses { get; set; }
		public MeAnotoContext(DbContextOptions<MeAnotoContext> options) : base(options) {
		}
		protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder);
	}
}
