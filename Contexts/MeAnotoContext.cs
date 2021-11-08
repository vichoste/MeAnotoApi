using MeAnotoApi.Models.Entities;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Contexts {
	public class MeAnotoContext : IdentityDbContext<ApplicationUser> {
		public DbSet<Attendee> Attendees { get; set; }
		public DbSet<Professor> Professors { get; set; }
		public DbSet<Institution> Institutions { get; set; }
		public DbSet<CampusSingular> CampusSingulars { get; set; }
		public DbSet<Career> Careers { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<CourseInstance> CourseInstances { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<EventInstance> EventInstances { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public MeAnotoContext(DbContextOptions<MeAnotoContext> options) : base(options) {
		}
		protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder);
	}
}
