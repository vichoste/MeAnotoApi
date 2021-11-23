using System.Collections.Generic;
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
[ApiController]
public class AttendeeController : ControllerBase { // TODO this entire thing
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
	[Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Attendee)]
	[HttpGet("{id}/" + Entities.EventInstance + "/" + Routes.All)]
	public async Task<ActionResult<IEnumerable<EventInstance>>> GetEventInstances(int id) {
		var attendee = await this._context.Attendees.FindAsync(id);
		return attendee is not null ? this.Ok(attendee.EventInstances) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
}
