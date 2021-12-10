using System;
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
/// Controller for institution
/// </summary>
[Authorize(Roles = UserRoles.Administrator), ApiController, EnableCors("FrontendCors"), Route(Routes.Api + "/" + Entities.Institution)]
public class InstitutionController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public InstitutionController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets an institution
	/// </summary>
	/// <param name="institutionId">Event ID</param>
	/// <returns>Event object in JSON format</returns>
	[HttpGet("{institutionId}")]
	public ActionResult<EntityResponse> GetInstitution(int institutionId) {
		try {
			var data =
				from i in this._context.Institutions
				where i.Id == institutionId
				select new EntityResponse {
					Id = i.Id,
					Name = i.Name
				};
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets all the institutions
	/// </summary>
	/// <returns>List of institutions in JSON format</returns>
	[HttpGet(Routes.All)]
	public ActionResult<IQueryable<EntityResponse>> ListInstitutions() {
		try {
			var data =
				from i in this._context.Institutions
				select new EntityResponse {
					Id = i.Id,
					Name = i.Name
				};
			return this.Ok(data);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Creates an institution
	/// </summary>
	/// <param name="institution">Institution</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[HttpPost]
	public async Task<ActionResult<Institution>> CreateInstitution(Institution institution) {
		try {
			var existing = await this._context.Institutions.FirstOrDefaultAsync(i => i.Name == institution.Name);
			if (existing is not null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
			}
			_ = this._context.Institutions.Add(institution);
			_ = await this._context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk, EntityResponse = new EntityResponse { Id = institution.Id, Name = institution.Name } });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
}
