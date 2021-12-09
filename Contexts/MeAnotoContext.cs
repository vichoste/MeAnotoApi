using MeAnotoApi.Models.Entities;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Contexts;
/// <summary>
/// Database context
/// </summary>
public class MeAnotoContext : IdentityDbContext<ApplicationUser> {
	/// <summary>
	/// Attendees
	/// </summary>
	public DbSet<Attendee> Attendees { get; set; }
	/// <summary>
	/// Professors
	/// </summary>
	public DbSet<Professor> Professors { get; set; }
	/// <summary>
	/// Institutions
	/// </summary>
	public DbSet<Institution> Institutions { get; set; }
	/// <summary>
	/// Events
	/// </summary>
	public DbSet<Event> Events { get; set; }
	/// <summary>
	/// Event instances
	/// </summary>
	public DbSet<EventInstance> EventInstances { get; set; }
	/// <summary>
	/// Creates a database context
	/// </summary>
	/// <param name="options">Options associated with this context</param>
	public MeAnotoContext(DbContextOptions<MeAnotoContext> options) : base(options) {
	}
	/// <summary>
	/// Model creation
	/// </summary>
	/// <param name="builder">Builder associated with this model</param>
	protected override void OnModelCreating(ModelBuilder builder) => base.OnModelCreating(builder);
}
