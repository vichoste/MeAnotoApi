using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for room
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.Room)]
public class RoomController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public RoomController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the rooms
	/// </summary>
	/// <returns>List of rooms in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<Room>>> Get() => await this._context.Rooms.ToListAsync();
	/// <summary>
	/// Gets a room
	/// </summary>
	/// <param name="id">Room ID</param>
	/// <returns>Room object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<Room>> Get(int id) {
		var entity = await this._context.Rooms.FindAsync(id);
		return entity is not null
			? this.Ok(entity)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates a room
	/// </summary>
	/// <param name="entity">Room</param>
	/// <param name="campusId">Campus ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{campusId}")]
	public async Task<ActionResult<Response>> Post(Room entity, int campusId) {
		var campusSingular = await this._context.CampusSingulars.FindAsync(campusId);
		if (campusSingular is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.CampusSingular = campusSingular;
		_ = this._context.Rooms.Add(entity);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
	/// <summary>
	/// Updates a room
	/// </summary>
	/// <param name="roomId">Room ID</param>
	/// <param name="capacity">New capacity</param>
	/// <returns>OK if updated successfully</returns>
	[Authorize(Roles = UserRoles.Manager)]
	[HttpPatch(Routes.Update + "/{roomId}/{capacity}")]
	public async Task<ActionResult<Response>> Update(int roomId, int capacity) {
		var entity = await this._context.Rooms.FindAsync(roomId);
		if (entity is not null) {
			entity.Capacity = capacity;
			_ = await this._context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.UpdatedOk });
		}
		return this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
}
