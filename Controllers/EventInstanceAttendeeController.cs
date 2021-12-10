using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MeAnotoApi.Contexts;
using MeAnotoApi.Information;
using MeAnotoApi.Strings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for event instance aimed at attendees
/// </summary>
[Authorize(Roles = UserRoles.Attendee), ApiController, EnableCors("FrontendCors"), Route(Routes.Api + "/" + UserRoles.Attendee + "/" + Entities.EventInstance)]
public class EventInstanceAttendeeController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public EventInstanceAttendeeController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the event instances owned by a attendee
	/// </summary>
	/// <returns>List of owned events in JSON format</returns>
	[HttpGet(Routes.All)]
	public ActionResult<IEnumerable<EntityResponse>> ListAttendeeEventInstances() {
		try {
			var name = this.HttpContext.User.Identity.Name;
			var attendee = this._context.Attendees.First(p => p.UserName == name);
			if (attendee is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var data =
				from a in this._context.Attendees
				from ei in this._context.EventInstances
				where ei.Attendees.Contains(a)
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
	/// Enrolls an attendee into an event instance
	/// </summary>
	/// <param name="eventInstanceId">Event instance ID</param>
	/// <returns>OK if enrolled successfully</returns>
	[HttpPost("{eventInstanceId}/" + Routes.Enroll)]
	public async Task<ActionResult<Response>> EnrollAttendee(int eventInstanceId) {
		try {
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
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
}
