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
	/// Gets the amount of attendees on a event instance
	/// </summary>
	/// <param name="eventId">Event instance ID</param>
	/// <returns>attendees in JSON format</returns>
	[HttpGet("{eventId}/" + UserRoles.Attendee + "/" + Routes.Count)]
	public ActionResult<IQueryable<int>> GetAttendeeCount(int eventId) {
		try {
			var data =
				from e in this._context.Events
				where e.Id == eventId
				select e.Attendees.Count;
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets a single event
	/// </summary>
	/// <param name="eventId">Event ID</param>
	/// <returns>Event in JSON format</returns>
	[HttpGet("{eventId}")]
	public ActionResult<IQueryable<Event>> GetEvent(int eventId) {
		try {
			var data =
				from e in this._context.Events
				where e.Id == eventId
				select e;
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
	public ActionResult<IQueryable<Event>> ListEvents() {
		try {
			var data =
				from e in this._context.Events
				select e;
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets all the events owned by a attendee
	/// </summary>
	/// <returns>List of owned events in JSON format</returns>
	[Authorize(Roles = UserRoles.Attendee), HttpGet(UserRoles.Attendee + "/" + Routes.All)]
	public ActionResult<IEnumerable<Event>> ListAttendeeEvents() {
		try {
			var name = this.HttpContext.User.Identity.Name;
			var attendee = this._context.Attendees.First(p => p.UserName == name);
			if (attendee is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var data =
				from a in this._context.Attendees
				from e in this._context.Events
				where e.Attendees.Contains(a)
				select e;
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets all the events owned by a professor
	/// </summary>
	/// <returns>List of the professor's events in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor), HttpGet(UserRoles.Professor + "/" + Routes.All)]
	public ActionResult<IQueryable<Event>> ListProfessorEvents() {
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
				select e;
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
	[Authorize(Roles = UserRoles.Professor), HttpPost]
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
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Enrolls an attendee into an event instance
	/// </summary>
	/// <param name="eventId">Event instance ID</param>
	/// <returns>OK if enrolled successfully</returns>
	[Authorize(Roles = UserRoles.Attendee), HttpPost(UserRoles.Attendee + "/{eventId}/" + Routes.Enroll)]
	public async Task<ActionResult<Response>> EnrollAttendee(int eventId) {
		try {
			var name = this.HttpContext.User.Identity.Name;
			var attendee = this._context.Attendees.First(p => p.UserName == name);
			if (attendee is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var @event = await this._context.Events.FindAsync(eventId);
			if (@event is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			if (@event.Capacity <= 0) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.EventFull });
			}
			if (@event.Attendees.Any(a => a.UserName == attendee.UserName)) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.AlreadyAttending });
			}
			@event.Capacity--;
			attendee.Events.Add(@event);
			@event.Attendees.Add(attendee);
			_ = await this._context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.EnrolledOk });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
}
