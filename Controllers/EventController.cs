using System;
using System.Linq;
using System.Threading.Tasks;

using MeAnotoApi.Contexts;
using MeAnotoApi.Information;
using MeAnotoApi.Models.Entities;
using MeAnotoApi.Strings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for event
/// </summary>
[ApiController, EnableCors("FrontendCors"), Route(Routes.Api + "/" + Entities.Event)]
public class EventController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public EventController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets a single event
	/// </summary>
	/// <param name="eventId">Event ID</param>
	/// <returns>Event in JSON format</returns>
	[HttpGet("{eventId}")]
	public ActionResult<IQueryable<EntityResponse>> GetEvent(int eventId) {
		try {
			var data =
				from e in this._context.Events
				where e.Id == eventId
				select new EntityResponse {
					Id = e.Id,
					Name = e.Name,
					Owner = e.Professor.UserName
				};
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets all the events
	/// </summary>
	/// <returns>List of events in JSON format</returns>
	[HttpGet(Routes.All)]
	public ActionResult<IQueryable<EntityResponse>> ListEvents() {
		try {
			var data =
				from e in this._context.Events
				select new EntityResponse {
					Id = e.Id,
					Name = e.Name,
					Owner = e.Professor.UserName
				};
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets all the events owned by a professor
	/// </summary>
	/// <returns>List of the professor's events in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor), HttpGet(Routes.All + "/" + UserRoles.Professor)]
	public ActionResult<IQueryable<EntityResponse>> ListProfessorEvents() {
		try {
			var name = this.HttpContext.User.Identity.Name;
			var professor = this._context.Professors.First(p => p.UserName == name);
			if (professor is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var data =
				from p in this._context.Professors
				join e in this._context.Events
				on p equals e.Professor
				where p == professor
				select new EntityResponse {
					Id = e.Id,
					Name = e.Name,
					Owner = e.Professor.UserName
				};
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Creates an event
	/// </summary>
	/// <param name="event">Event</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor), HttpPost("{institutionId}")]
	public async Task<ActionResult<Response>> CreateEvent(Event @event) {
		try {
			var existing = await this._context.Events.FirstOrDefaultAsync(e => e.Name == @event.Name);
			if (existing is not null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
			}
			var name = this.HttpContext.User.Identity.Name;
			var professor = this._context.Professors.First(p => p.UserName == name);
			if (professor is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			@event.Institution = professor.Institution;
			@event.Professor = professor;
			professor.Events.Add(@event);
			_ = this._context.Events.Add(@event);
			_ = await this._context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk, EntityResponse = new EntityResponse { Id = @event.Id, Name = @event.Name, Owner = professor.UserName } });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
}
