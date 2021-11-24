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
/// Controller for campuses. It has "singular" because of the fucking naming system EF has.
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.CampusSingular)]
public class CampusSingularController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public CampusSingularController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the campuses
	/// </summary>
	/// <returns>List of all the campuses in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<CampusSingular>>> Get() => await this._context.CampusSingulars.ToListAsync();
	/// <summary>
	/// Gets a campus
	/// </summary>
	/// <param name="id">Campus ID</param>
	/// <returns>Campus object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<CampusSingular>> Get(int id) {
		var campusSingular = await this._context.CampusSingulars.FindAsync(id);
		return campusSingular is not null
			? this.Ok(campusSingular)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates a campus
	/// </summary>
	/// <param name="campusSingular">Campus</param>
	/// <param name="institutionId">Institution ID</param>
	/// <returns>OK if created successfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{institutionId}")]
	public async Task<ActionResult<CampusSingular>> Post(CampusSingular campusSingular, int institutionId) {
		var institution = await this._context.Institutions.FindAsync(institutionId);
		if (institution is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		campusSingular.Institution = institution;
		_ = this._context.CampusSingulars.Add(campusSingular);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
