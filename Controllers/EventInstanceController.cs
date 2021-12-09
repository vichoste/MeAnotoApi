using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MeAnotoApi.Contexts;
using MeAnotoApi.Information;
using MeAnotoApi.Models.Entities;
using MeAnotoApi.Strings;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for event instance
/// </summary>
[ApiController, Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.Professor), EnableCors("FrontendCors"), Route(Routes.Api + "/" + Entities.EventInstance)]
public class EventInstanceController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public EventInstanceController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets the amount of attendees on a event instance
	/// </summary>
	/// <param name="eventInstanceId">Event instance ID</param>
	/// <returns>attendees in JSON format</returns>
	[HttpGet("{eventInstanceId}/" + UserRoles.Attendee + "/" + Routes.Count)]
	public async Task<ActionResult<Response>> GetAttendeeCount(int eventInstanceId) {
		var eventInstance = await this._context.EventInstances.FindAsync(eventInstanceId);
		if (eventInstance is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		return professor is null
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: eventInstance.Event.Professor != professor
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: this.Ok(new Response { Status = Statuses.Ok, Message = eventInstance.Attendees.Count.ToString() });
	}
	/// <summary>
	/// Gets an event instance owned by the current professor
	/// </summary>
	/// <param name="eventInstanceId">Event instance ID</param>
	/// <returns>Event instance object in JSON format</returns>
	[HttpGet(UserRoles.Professor + "/{eventInstanceId}")]
	public async Task<ActionResult<EntityResponse>> GetEventInstance(int eventInstanceId) {
		var eventInstance = await this._context.EventInstances.FindAsync(eventInstanceId);
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		return professor is null
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: !(eventInstance.Event.Professor == professor)
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: eventInstance is not null ? this.Ok(new EntityResponse { Id = eventInstance.Id, Name = eventInstance.Name, Owner = professor.UserName })
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Gets the event instances associated with an attendee
	/// </summary>
	/// <returns>Event instances associated with an attendee in JSON format</returns>
	[Authorize(Roles = UserRoles.Attendee)]
	[HttpGet(UserRoles.Attendee + "/" + Entities.EventInstance + "/" + Routes.All)]
	public ActionResult<IEnumerable<EventInstance>> ListAttendeeEventInstances() {
		var name = this.HttpContext.User.Identity.Name;
		var attendee = this._context.Attendees.First(p => p.UserName == name);
		return attendee is not null
			? this.Ok(attendee.EventInstances)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Gets all the event instances owned by the current professor
	/// </summary>
	/// <returns>List of owned events in JSON format</returns>
	[HttpGet(UserRoles.Professor + "/" + Entities.EventInstance + "/" + Routes.All)]
	public async Task<ActionResult<IEnumerable<EntityResponse>>> ListProfessorEventInstances() {
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var myEventInstances = this._context.EventInstances.Where(e => e.Event.Professor == professor);
		var eventInstances = await myEventInstances.ToListAsync();
		var response = new List<EntityResponse>();
		foreach (var eventInstance in eventInstances) {
			response.Add(new EntityResponse { Id = eventInstance.Id, Name = eventInstance.Name, Owner = professor.UserName });
		}
		return response;
	}
	/// <summary>
	/// Creates an event instance
	/// </summary>
	/// <param name="eventInstance">Event instance</param>
	/// <param name="eventId">Event ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[HttpPost("{eventId}")]
	public async Task<ActionResult<EventInstance>> CreateEventInstance(EventInstance eventInstance, int eventId) {
		var existing = await this._context.EventInstances.FirstOrDefaultAsync(e => e.Name == eventInstance.Name);
		if (existing is not null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
		}
		var @event = await this._context.Events.FindAsync(eventId);
		if (@event is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		if (@event.Professor != professor) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		eventInstance.Event = @event;
		_ = this._context.EventInstances.Add(eventInstance);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(eventInstance);
	}
	/// <summary>
	/// Enrolls an attendee into an event instance
	/// </summary>
	/// <param name="eventInstanceId">Event instance ID</param>
	/// <returns>OK if enrolled successfully</returns>
	[HttpPost("{eventInstanceId}/" + Routes.Enroll + "/" + UserRoles.Attendee)]
	public async Task<ActionResult<Response>> EnrollAttendee(int eventInstanceId) {
		var name = this.HttpContext.User.Identity.Name;
		var attendee = this._context.Attendees.First(p => p.UserName == name);
		if (attendee is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var eventInstance = await this._context.EventInstances.FindAsync(eventInstanceId);
		if (eventInstance is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		eventInstance.Attendees.Add(attendee);
		attendee.EventInstances.Add(eventInstance);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.EnrolledOk });
	}
}
