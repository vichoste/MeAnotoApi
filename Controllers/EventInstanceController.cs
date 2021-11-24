using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for event instance
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.EventInstance)]
public class EventInstanceController : ControllerBase {
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IConfiguration _configuration;
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="userManager">User manager</param>
	/// <param name="roleManager">Role manager</param>
	/// <param name="configuration">Configuration</param>
	/// <param name="context">Database context</param>
	public EventInstanceController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, MeAnotoContext context) {
		this._userManager = userManager;
		this._roleManager = roleManager;
		this._configuration = configuration;
		this._context = context;
	}
	/// <summary>
	/// Gets all the event instances
	/// </summary>
	/// <returns>List of event instances in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<EventInstance>>> Get() => await this._context.EventInstances.ToListAsync();
	/// <summary>
	/// Gets an event instance
	/// </summary>
	/// <param name="id">Event instance ID</param>
	/// <returns>Event instance object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<EventInstance>> Get(int id) {
		var entity = await this._context.EventInstances.FindAsync(id);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates an event instance
	/// </summary>
	/// <param name="entity">Event instance</param>
	/// <param name="eventId">Event ID</param>
	/// <param name="courseInstanceId">Course instance ID</param>
	/// <param name="roomId">Room ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor)]
	[HttpPost("{eventId}/{courseInstanceId}/{roomId}")]
	public async Task<ActionResult<EventInstance>> Post(EventInstance entity, int eventId, int courseInstanceId, int roomId) {
		var @event = await this._context.Events.FindAsync(eventId);
		var courseInstance = await this._context.CourseInstances.FindAsync(courseInstanceId);
		var room = await this._context.Rooms.FindAsync(roomId);
		if (@event is null || courseInstance is null || room is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.Event = @event;
		entity.CourseInstance = courseInstance;
		entity.Room = room;
		_ = this._context.EventInstances.Add(entity);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
