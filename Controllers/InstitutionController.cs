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
/// Controller for institution
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.Institution)]
public class InstitutionController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public InstitutionController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the institutions
	/// </summary>
	/// <returns>List of institutions in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<EntityResponse>>> Get() {
		var institutions = await this._context.Institutions.ToListAsync();
		var response = new List<EntityResponse>();
		foreach (var institution in institutions) {
			response.Add(new EntityResponse { Id = institution.Id, Name = institution.Name });
		}
		return response;
	}
	/// <summary>
	/// Gets an institution
	/// </summary>
	/// <param name="id">Event ID</param>
	/// <returns>Event object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<EntityResponse>> Get(int id) {
		var institution = await this._context.Institutions.FindAsync(id);
		return institution is not null
			? this.Ok(new EntityResponse { Id = institution.Id, Name = institution.Name })
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates an institution
	/// </summary>
	/// <param name="institution">Institution</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost]
	public async Task<ActionResult<Institution>> Post(Institution institution) {
		var existing = await this._context.Institutions.FirstOrDefaultAsync(i => i.Name == institution.Name);
		if (existing is not null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
		}
		_ = this._context.Institutions.Add(institution);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(institution);
	}
}
