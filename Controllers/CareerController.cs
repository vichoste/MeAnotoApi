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
[Route(Routes.Api + "/" + Entities.Career)]
public class CareerController : ControllerBase {
	private readonly MeAnotoContext _Context;
	public CareerController(MeAnotoContext context) => this._Context = context;
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<Career>>> Get() => await this._Context.Careers.ToListAsync();
	[HttpGet("{id}")]
	public async Task<ActionResult<Career>> Get(int id) {
		var entity = await this._Context.Careers.FindAsync(id);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{" + Entities.CampusSingular + "}")]
	public async Task<ActionResult<Career>> Post(Career entity, int campusSingulerId) {
		var campusSingular = await this._Context.CampusSingulars.FindAsync(campusSingulerId);
		if (campusSingular is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.CampusSingular = campusSingular;
		_ = this._Context.Careers.Add(entity);
		_ = await this._Context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
