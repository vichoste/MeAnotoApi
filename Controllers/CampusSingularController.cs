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
[Route(Routes.Api + "/" + Entities.CampusSingular)]
public class CampusSingularController : ControllerBase {
	private readonly MeAnotoContext _Context;
	public CampusSingularController(MeAnotoContext context) => this._Context = context;
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<CampusSingular>>> Get() => await this._Context.CampusSingulars.ToListAsync();
	[HttpGet("{" + Entities.CampusSingular + "}")]
	public async Task<ActionResult<CampusSingular>> Get(int id) {
		var entity = await this._Context.CampusSingulars.FindAsync(id);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{" + Entities.Institution + "}")]
	public async Task<ActionResult<CampusSingular>> Post(CampusSingular entity, int institutionId) {
		var institution = await this._Context.Institutions.FindAsync(institutionId);
		if (institution is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.Institution = institution;
		_ = this._Context.CampusSingulars.Add(entity);
		_ = await this._Context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
