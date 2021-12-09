//using System.Collections.Generic;
//using System.Threading.Tasks;

//using MeAnotoApi.Authentication;
//using MeAnotoApi.Contexts;
//using MeAnotoApi.Models.Entities;

//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace MeAnotoApi.Controllers;
///// <summary>
///// Controller for campuses. It has "singular" because of the fucking naming system EF has.
///// </summary>
//[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//[EnableCors("FrontendCors")]
//[Route(Routes.Api + "/" + Entities.CampusSingular)]
//public class CampusSingularController : ControllerBase {
//	private readonly MeAnotoContext _context;
//	/// <summary>
//	/// Creates the controller
//	/// </summary>
//	/// <param name="context">Database context</param>
//	public CampusSingularController(MeAnotoContext context) => this._context = context;
//	/// <summary>
//	/// Gets all the campusSingulars
//	/// </summary>
//	/// <returns>List of campusSingulars in JSON format</returns>
//	[HttpGet(Routes.All)]
//	public async Task<ActionResult<IEnumerable<EntityResponse>>> Get() {
//		var campusSingulars = await this._context.CampusSingulars.ToListAsync();
//		var response = new List<EntityResponse>();
//		foreach (var campusSingular in campusSingulars) {
//			response.Add(new EntityResponse { Id = campusSingular.Id, Name = campusSingular.Name });
//		}
//		return response;
//	}
//	/// <summary>
//	/// Gets an campusSingular
//	/// </summary>
//	/// <param name="id">Event ID</param>
//	/// <returns>Event object in JSON format</returns>
//	[HttpGet("{id}")]
//	public async Task<ActionResult<EntityResponse>> Get(int id) {
//		var campusSingular = await this._context.CampusSingulars.FindAsync(id);
//		return campusSingular is not null
//			? this.Ok(new EntityResponse { Id = campusSingular.Id, Name = campusSingular.Name })
//			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
//	}
//	/// <summary>
//	/// Creates a campus
//	/// </summary>
//	/// <param name="campusSingular">Campus</param>
//	/// <param name="institutionId">Institution ID</param>
//	/// <returns>OK if created successfully in JSON format</returns>
//	[Authorize(Roles = UserRoles.Administrator)]
//	[HttpPost("{institutionId}")]
//	public async Task<ActionResult<CampusSingular>> Post(CampusSingular campusSingular, int institutionId) {
//		var existing = await this._context.CampusSingulars.FirstOrDefaultAsync(c => c.Name == campusSingular.Name);
//		if (existing is not null) {
//			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
//		}
//		var institution = await this._context.Institutions.FindAsync(institutionId);
//		if (institution is null) {
//			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
//		}
//		campusSingular.Institution = institution;
//		_ = this._context.CampusSingulars.Add(campusSingular);
//		_ = await this._context.SaveChangesAsync();
//		return this.Ok(campusSingular);
//	}
//}
