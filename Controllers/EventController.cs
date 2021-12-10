﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MeAnotoApi.Contexts;
using MeAnotoApi.Information;
using MeAnotoApi.Models.Entities;
using MeAnotoApi.Strings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for event
/// </summary>
[Authorize(Roles = UserRoles.Professor), ApiController, EnableCors("FrontendCors"), Route(Routes.Api + "/" + Entities.Event)]
public class EventController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public EventController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the events owned by the current professor
	/// </summary>
	/// <returns>List of owned events in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<EntityResponse>>> GetEvent() {
		try {
			var name = this.HttpContext.User.Identity.Name;
			var professor = this._context.Professors.First(p => p.UserName == name);
			if (professor is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var myEvents = this._context.Events.Where(e => e.Professor == professor);
			var events = await myEvents.ToListAsync();
			var response = new List<EntityResponse>();
			foreach (var @event in events) {
				response.Add(new EntityResponse { Id = @event.Id, Name = @event.Name, Owner = professor.UserName });
			}
			return response;
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets an event owned by the current professor
	/// </summary>
	/// <param name="eventId">Course ID</param>
	/// <returns>Course object in JSON format</returns>
	[HttpGet("{eventId}")]
	public async Task<ActionResult<EntityResponse>> GetEvent(int eventId) {
		try {
			var @event = await this._context.Events.FindAsync(eventId);
			var name = this.HttpContext.User.Identity.Name;
			var professor = this._context.Professors.First(p => p.UserName == name);
			return professor is null
				? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
				: !(@event.Professor == professor)
				? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
				: @event is not null ? this.Ok(new EntityResponse { Id = @event.Id, Name = @event.Name, Owner = professor.UserName })
				: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Creates an event
	/// </summary>
	/// <param name="event">Event</param>
	/// <param name="institutionId">Institution ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[HttpPost("{institutionId}")]
	public async Task<ActionResult<Event>> CreateEvent(Event @event, int institutionId) {
		try {
			var existing = await this._context.Events.FirstOrDefaultAsync(e => e.Name == @event.Name);
			if (existing is not null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
			}
			var institution = await this._context.Institutions.FindAsync(institutionId);
			if (institution is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var name = this.HttpContext.User.Identity.Name;
			var professor = this._context.Professors.First(p => p.UserName == name);
			if (professor is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			@event.Institution = institution;
			@event.Professor = professor;
			_ = this._context.Events.Add(@event);
			_ = await this._context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk, EntityResponse = new EntityResponse { Id = @event.Id, Name = @event.Name, Owner = @event.Professor.UserName } });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
}
