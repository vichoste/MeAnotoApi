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
[Route(Routes.Api + "/" + Entities.Room)]
public class RoomController : ControllerBase {
	private readonly MeAnotoContext _Context;
	public RoomController(MeAnotoContext context) => this._Context = context;
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<Room>>> Get() => await this._Context.Rooms.ToListAsync();
	[HttpGet("{" + Entities.Room + "}")]
	public async Task<ActionResult<Room>> Get(int roomId) {
		var entity = await this._Context.Rooms.FindAsync(roomId);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{" + Entities.CampusSingular + "}")]
	public async Task<ActionResult<Room>> Post(Room entity, int campus) {
		var campusSingular = await this._Context.CampusSingulars.FindAsync(campus);
		if (campusSingular is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.CampusSingular = campusSingular;
		_ = this._Context.Rooms.Add(entity);
		_ = await this._Context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
	[Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager)]
	[HttpPatch(Routes.Update + "/{" + Entities.Room + "}/{" + JsonPropertyNames.Capacity + "}")]
	public async Task<ActionResult<Room>> Update(int roomId, int capacity) {
		var entity = await this._Context.Rooms.FindAsync(roomId);
		if (entity is not null) {
			entity.Capacity = capacity;
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.UpdatedOk });
		}
		return this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
}
