using System;
using System.Collections.Generic;
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
/// Controller for event instance aimed at professors
/// </summary>
[ApiController, EnableCors("FrontendCors"), Route(Routes.Api + "/" + UserRoles.Professor + "/" + Entities.EventInstance)]
public class EventInstanceProfessorController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public EventInstanceProfessorController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets the amount of attendees on a event instance
	/// </summary>
	/// <param name="eventInstanceId">Event instance ID</param>
	/// <returns>attendees in JSON format</returns>
	[HttpGet("{eventInstanceId}/" + UserRoles.Attendee + "/" + Routes.Count)]
	public ActionResult<IQueryable<int>> GetAttendeeCount(int eventInstanceId) {
		try {
			var data =
				from ei in this._context.EventInstances
				where ei.Id == eventInstanceId
				select ei.Attendees.Count;
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets a single event instance
	/// </summary>
	/// <param name="eventInstanceId">Event instance ID</param>
	/// <returns>Event instance object in JSON format</returns>
	[HttpGet("{eventInstanceId}")]
	public ActionResult<IQueryable<EntityResponse>> GetEventInstance(int eventInstanceId) {
		try {
			var data =
				from ei in this._context.EventInstances
				where ei.Id == eventInstanceId
				select new EntityResponse {
					Id = ei.Id,
					Name = ei.Name,
					Owner = ei.Event.Professor.UserName
				};
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets all the event instances
	/// </summary>
	/// <returns>List of owned events in JSON format</returns>
	[HttpGet(Routes.All)]
	public ActionResult<IEnumerable<EntityResponse>> ListEventInstances() {
		try {
			var data =
				from e in this._context.Events
				join ei in this._context.EventInstances
				on e.Id equals ei.Event.Id
				select new EntityResponse {
					Id = ei.Id,
					Name = ei.Name,
					Owner = ei.Event.Professor.UserName
				};
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets all the event instances owned by a professor
	/// </summary>
	/// <returns>List of owned events in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor), HttpGet(Routes.All + "/{professorId}")]
	public ActionResult<IEnumerable<EntityResponse>> ListProfessorEventInstances() { // TODO This wea is bringing everything
		try {
			var name = this.HttpContext.User.Identity.Name;
			var professor = this._context.Professors.First(p => p.UserName == name);
			if (professor is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var data =
				from ei in this._context.EventInstances
				from p in this._context.Professors
				join e in this._context.Events
				on ei.Event.Id equals e.Id
				where e.Professor.Id == p.Id
				select new EntityResponse {
					Id = ei.Id,
					Name = ei.Name,
					Owner = ei.Event.Professor.UserName
				};
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Creates an event instance
	/// </summary>
	/// <param name="eventInstance">Event instance</param>
	/// <param name="eventId">Event ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor), HttpPost("{eventId}")]
	public async Task<ActionResult<EventInstance>> CreateEventInstance(EventInstance eventInstance, int eventId) {
		try {
			var name = this.HttpContext.User.Identity.Name;
			var professor = this._context.Professors.First(p => p.UserName == name);
			if (professor is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var existing = await this._context.EventInstances.FirstOrDefaultAsync(e => e.Name == eventInstance.Name);
			if (existing is not null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
			}
			var data = await
				(from e in this._context.Events
				 join p in this._context.Professors
				 on e.Professor.Id equals p.Id
				 where p.UserName == professor.UserName
				 select p).ToListAsync();
			if (data.Count == 0) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var @event = await this._context.Events.FindAsync(eventId);
			if (@event is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			eventInstance.Event = @event;
			_ = this._context.EventInstances.Add(eventInstance);
			_ = await this._context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk, EntityResponse = new EntityResponse { Id = eventInstance.Id, Name = eventInstance.Name, Owner = professor.UserName } });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
}
