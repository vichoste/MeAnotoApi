using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for event
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.Event)]
public class EventController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public EventController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the events owned by the current professor
	/// </summary>
	/// <returns>List of owned events in JSON format</returns>
	[HttpGet(Routes.All)]
	public ActionResult<IEnumerable<Event>> Get() {
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var myEvents = this._context.Events.Where(c => c.Professor == professor);
		return this.Ok(myEvents);
	}
	/// <summary>
	/// Gets an event owned by the current professor
	/// </summary>
	/// <param name="id">Course ID</param>
	/// <returns>Course object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<Event>> Get(int id) {
		var @event = await this._context.Events.FindAsync(id);
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		return professor is null
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: !(@event.Professor == professor)
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: @event is not null ? this.Ok(@event)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates an event
	/// </summary>
	/// <param name="event">Event</param>
	/// <param name="institutionId">Institution ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor)]
	[HttpPost("{institutionId}")]
	public async Task<ActionResult<Event>> Post(Event @event, int institutionId) {
		var existing = await this._context.Events.FirstOrDefaultAsync(e => e.Name == @event.Name);
		if (existing is not null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
		}
		var institution = await this._context.Institutions.FindAsync(institutionId);
		if (institution is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		@event.Institution = institution;
		@event.Professor = professor;
		_ = this._context.Events.Add(@event);
		_ = await this._context.SaveChangesAsync();
		System.Diagnostics.Debug.WriteLine($"Prof: {professor} - {@event.Professor.UserName}");
		return this.Ok(@event);
	}
}
