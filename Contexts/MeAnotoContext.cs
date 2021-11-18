using MeAnotoApi.Models.Entities;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Contexts;

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
	protected override void OnModelCreating(ModelBuilder builder) {
		base.OnModelCreating(builder);
		_ = builder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(32));
		_ = builder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedEmail).HasMaxLength(32));
		_ = builder.Entity<ApplicationUser>(entity => entity.Property(m => m.UserName).HasMaxLength(32));
		_ = builder.Entity<ApplicationUser>(entity => entity.Property(m => m.Email).HasMaxLength(32));
		_ = builder.Entity<ApplicationUser>(entity => entity.Property(m => m.NormalizedUserName).HasMaxLength(32));
		_ = builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(32));
		_ = builder.Entity<IdentityRole>(entity => entity.Property(m => m.Name).HasMaxLength(32));
		_ = builder.Entity<IdentityRole>(entity => entity.Property(m => m.NormalizedName).HasMaxLength(32));
		_ = builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(32));
		_ = builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(32));
		_ = builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(32));
		_ = builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(32));
		_ = builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(32));
		_ = builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(32));
		_ = builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(32));
		_ = builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(32));
		_ = builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(32));
		_ = builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(32));
		_ = builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(32));
		_ = builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(32));
	}
}
