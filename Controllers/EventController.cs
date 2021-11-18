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
[Route(Routes.Api + "/" + Entities.Event)]
public class EventController : ControllerBase {
	private readonly MeAnotoContext _context;
	public EventController(MeAnotoContext context) => this._context = context;
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<Event>>> Get() => await this._context.Events.ToListAsync();
	[HttpGet("{id}")]
	public async Task<ActionResult<Event>> Get(int id) {
		var entity = await this._context.Events.FindAsync(id);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	[Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Professor)]
	[HttpPost("{institutionId}")]
	public async Task<ActionResult<Event>> Post(Event entity, int institutionId) {
		var institution = await this._context.Institutions.FindAsync(institutionId);
		if (institution is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.Institution = institution;
		_ = this._context.Events.Add(entity);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
