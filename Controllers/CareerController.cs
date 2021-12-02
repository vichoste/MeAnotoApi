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
/// Controller for career
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.Career)]
public class CareerController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database controller</param>
	public CareerController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all careers
	/// </summary>
	/// <returns>List of all careers in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<Career>>> Get() => await this._context.Careers.ToListAsync();
	/// <summary>
	/// Gets a career
	/// </summary>
	/// <param name="id">Career ID</param>
	/// <returns>Career object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<Career>> Get(int id) {
		var career = await this._context.Careers.FindAsync(id);
		return career is not null
			? this.Ok(career)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates a career
	/// </summary>
	/// <param name="career">Career</param>
	/// <param name="campusSingularId">Campus ID</param>
	/// <returns>OK if created successfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{campusSingularId}")]
	public async Task<ActionResult<Response>> Post(Career career, int campusSingularId) {
		var campusSingular = await this._context.CampusSingulars.FindAsync(campusSingularId);
		if (campusSingular is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		career.CampusSingular = campusSingular;
		_ = this._context.Careers.Add(career);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
