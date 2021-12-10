using System;
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
/// Controller for event instance
/// </summary>
[ApiController, EnableCors("FrontendCors"), Route(Routes.Api + "/" + UserRoles.Attendee + "/" + Entities.EventInstance)]
public class EventInstanceAttendeeController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public EventInstanceAttendeeController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Enrolls an attendee into an event instance
	/// </summary>
	/// <param name="eventInstanceId">Event instance ID</param>
	/// <returns>OK if enrolled successfully</returns>
	[Authorize(Roles = UserRoles.Attendee), HttpPost("{eventInstanceId}/" + Routes.Enroll)]
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
