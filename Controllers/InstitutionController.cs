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
[Route(Routes.Api + "/" + Entities.Institution)]
public class InstitutionController : ControllerBase {
	private readonly MeAnotoContext _Context;
	public InstitutionController(MeAnotoContext context) => this._Context = context;
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<Institution>>> Get() => await this._Context.Institutions.ToListAsync();
	[HttpGet("{" + Entities.Institution + "}")]
	public async Task<ActionResult<Institution>> Get(int institutionId) {
		var entity = await this._Context.Institutions.FindAsync(institutionId);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost]
	public async Task<ActionResult<Institution>> Post(Institution entity) {
		_ = this._Context.Institutions.Add(entity);
		_ = await this._Context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
