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
	[Route(Routes.Api + "/" + Entities.Event)]
	public class EventController : ControllerBase {
		private readonly MeAnotoContext _Context;
		public EventController(MeAnotoContext context) => this._Context = context;
		[HttpGet(Routes.All)]
		public async Task<ActionResult<IEnumerable<Event>>> Get() => await this._Context.Events.ToListAsync();
		[HttpGet("{id}")]
		public async Task<ActionResult<Event>> Get(int id) {
			var entity = await this._Context.Events.FindAsync(id);
			return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
		}
		[Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Professor)]
		[HttpPost("{" + Entities.Institution + "}")]
		public async Task<ActionResult<Event>> Post(Event entity, int institutionId) {
			var institution = await this._Context.Institutions.FindAsync(institutionId);
			if (institution is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			entity.Institution = institution;
			_ = this._Context.Events.Add(entity);
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
		}
	}
}