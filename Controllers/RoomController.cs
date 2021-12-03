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
	public async Task<ActionResult<IEnumerable<EntityResponse>>> Get() {
		var rooms = await this._context.Rooms.ToListAsync();
		var response = new List<EntityResponse>();
		foreach (var room in rooms) {
			response.Add(new EntityResponse { Id = room.Id, Name = room.Name });
		}
		return response;
	}
	/// <summary>
	/// Gets an room
	/// </summary>
	/// <param name="id">Event ID</param>
	/// <returns>Event object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<EntityResponse>> Get(int id) {
		var room = await this._context.Rooms.FindAsync(id);
		return room is not null
			? this.Ok(new EntityResponse { Id = room.Id, Name = room.Name })
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates a room
	/// </summary>
	/// <param name="room">Room</param>
	/// <param name="campusId">Campus ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{campusId}")]
	public async Task<ActionResult<Room>> Post(Room room, int campusId) {
		var existing = await this._context.Rooms.FirstOrDefaultAsync(r => r.Name == room.Name);
		if (existing is not null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
		}
		var campusSingular = await this._context.CampusSingulars.FindAsync(campusId);
		if (campusSingular is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		room.CampusSingular = campusSingular;
		_ = this._context.Rooms.Add(room);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(room);
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
