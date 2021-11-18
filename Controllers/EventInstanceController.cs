using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers;

[ApiController]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.EventInstance)]
public class EventInstanceController : ControllerBase {
	private readonly MeAnotoContext _context;
	public EventInstanceController(MeAnotoContext context) => this._context = context;
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<EventInstance>>> Get() => await this._context.EventInstances.ToListAsync();
	[HttpGet("{" + Entities.EventInstance + "}")]
	public async Task<ActionResult<EventInstance>> Get(int id) {
		var entity = await this._context.EventInstances.FindAsync(id);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	[Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Professor)]
	[HttpPost("{" + Entities.Event + "}/{" + Entities.CourseInstance + "}/{" + Entities.Room + "}")]
	public async Task<ActionResult<EventInstance>> Post(EventInstance entity, int eventId, int courseInstanceId, int roomId) {
		var @event = await this._context.Events.FindAsync(eventId);
		var courseInstance = await this._context.CourseInstances.FindAsync(courseInstanceId);
		var room = await this._context.Rooms.FindAsync(roomId);
		if (@event is null || courseInstance is null || room is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.Event = @event;
		entity.CourseInstance = courseInstance;
		entity.Room = room;
		_ = this._context.EventInstances.Add(entity);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
