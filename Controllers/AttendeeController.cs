using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for attendees
/// </summary>
[Route(Routes.Api + "/" + UserRoles.Attendee)]
[Authorize(Roles = UserRoles.Attendee)]
[ApiController]
public class AttendeeController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public AttendeeController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets the event instances associated with an attendee
	/// </summary>
	/// <param name="id">Attendee ID</param>
	/// <returns>Event instances associated with an attendee in JSON format</returns>
	[HttpGet("{id}/" + Entities.EventInstance + "/" + Routes.All)]
	public async Task<ActionResult<IEnumerable<EventInstance>>> GetEventInstances(int id) {
		var attendee = await this._context.Attendees.FindAsync(id);
		return attendee is not null
			? this.Ok(attendee.EventInstances)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Enrolls an attendee into an event instance
	/// </summary>
	/// <param name="id">Event instance ID</param>
	/// <returns>OK if enrolled successfully</returns>
	[HttpPost("{id}/" + Routes.Enroll + "/" + UserRoles.Attendee)]
	public async Task<ActionResult<Response>> EnrollAttendee(int id) {
		var name = this.HttpContext.User.Identity.Name;
		var attendee = this._context.Attendees.First(p => p.UserName == name);
		if (attendee is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var eventInstance = await this._context.EventInstances.FindAsync(id);
		if (eventInstance is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		eventInstance.Attendees.Add(attendee);
		attendee.EventInstances.Add(eventInstance);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.EnrolledOk });
	}
}
