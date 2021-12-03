﻿using System.Collections.Generic;
using System.Linq;
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
/// Controller for event instance
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.EventInstance)]
public class EventInstanceController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public EventInstanceController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the event instances owned by the current professor
	/// </summary>
	/// <returns>List of owned events in JSON format</returns>
	[HttpGet(Routes.All)]
	public ActionResult<IEnumerable<EventInstance>> Get() {
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var myEvents = this._context.EventInstances.Where(e => e.Event.Professor == professor);
		return this.Ok(myEvents);
	}
	/// <summary>
	/// Gets an event instance owned by the current professor
	/// </summary>
	/// <param name="id">Course ID</param>
	/// <returns>Course object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<EventInstance>> Get(int id) {
		var eventInstance = await this._context.EventInstances.FindAsync(id);
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		return professor is null
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: !(eventInstance.Event.Professor == professor)
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: eventInstance is not null ? this.Ok(eventInstance)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates an event instance
	/// </summary>
	/// <param name="eventInstance">Event instance</param>
	/// <param name="eventId">Event ID</param>
	/// <param name="courseInstanceId">Course instance ID</param>
	/// <param name="roomId">Room ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor)]
	[HttpPost("{eventId}/{courseInstanceId}/{roomId}")]
	public async Task<ActionResult<EventInstance>> Post(EventInstance eventInstance, int eventId, int courseInstanceId, int roomId) {
		var existing = await this._context.EventInstances.FirstOrDefaultAsync(e => e.Name == eventInstance.Name);
		if (existing is not null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
		}
		var @event = await this._context.Events.FindAsync(eventId);
		if (@event is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		if (@event.Professor != professor) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var courseInstance = await this._context.CourseInstances.FindAsync(courseInstanceId);
		var room = await this._context.Rooms.FindAsync(roomId);
		if (courseInstance is null || room is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		eventInstance.Event = @event;
		eventInstance.CourseInstance = courseInstance;
		eventInstance.Room = room;
		_ = this._context.EventInstances.Add(eventInstance);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(eventInstance);
	}
}
