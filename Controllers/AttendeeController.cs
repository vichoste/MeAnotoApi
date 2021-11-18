using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Mvc;

namespace MeAnotoApi.Controllers;
[Route(Routes.Api + "/" + UserRoles.Attendee)]
[ApiController]
public class AttendeeController : ControllerBase {
	private readonly MeAnotoContext _context;
	public AttendeeController(MeAnotoContext context) => this._context = context;
	[HttpGet("{id}/" + Entities.EventInstance + "/" + Routes.All)]
	public async Task<ActionResult<Career>> GetEventInstances(int id) {
		var attendee = await this._context.Attendees.FindAsync(id);
		return attendee is not null ? this.Ok(attendee.EventInstances) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
}
