using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers {
	[ApiController]
	[EnableCors("FrontendCors")]
	[Route(Routes.Api + "/" + Entities.Room)]
	public class RoomController : ControllerBase {
		private readonly MeAnotoContext _Context;
		public RoomController(MeAnotoContext context) => this._Context = context;
		[HttpGet(Routes.All)]
		public async Task<ActionResult<IEnumerable<Room>>> Get() => await this._Context.Rooms.ToListAsync();
		[HttpGet("{id}")]
		public async Task<ActionResult<Room>> Get(int id) {
			var entity = await this._Context.Rooms.FindAsync(id);
			return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
		}
		[Authorize(Roles = UserRoles.Administrator)]
		[HttpPost]
		public async Task<ActionResult<Room>> Post(Room entity) {
			_ = this._Context.Rooms.Add(entity);
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
		}
		[Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager)]
		[HttpPatch(Routes.Update + "/{id}/{capacidad}")]
		public async Task<ActionResult<Room>> Update(int id, int capacidad) {
			var entity = await this._Context.Rooms.FindAsync(id);
			if (entity is not null) {
				entity.Capacity = capacidad;
				_ = await this._Context.SaveChangesAsync();
				return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.UpdatedOk });
			}
			return this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
		}
	}
}