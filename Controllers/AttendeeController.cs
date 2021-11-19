using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeAnotoApi.Controllers;
[Route(Routes.Api + "/" + UserRoles.Attendee)]
[ApiController]
public class AttendeeController : ControllerBase {
	private readonly MeAnotoContext _context;
	public AttendeeController(MeAnotoContext context) => this._context = context;
	[Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Attendee)]
	[HttpGet("{id}/" + Entities.EventInstance + "/" + Routes.All)]
	public async Task<ActionResult<IEnumerable<EventInstance>>> GetEventInstances(int id) {
		var attendee = await this._context.Attendees.FindAsync(id);
		return attendee is not null ? this.Ok(attendee.EventInstances) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	public async Task<ActionResult<EventInstance>> PostReservation(int id) {
		var attendee = this._context.Attendees.FindAsync(id);
	}
}
