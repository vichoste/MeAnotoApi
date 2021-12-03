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
	/// Gets all the courses
	/// </summary>
	/// <returns>List of courses in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<EntityResponse>>> Get() {
		var courses = await this._context.Courses.ToListAsync();
		var response = new List<EntityResponse>();
		foreach (var course in courses) {
			response.Add(new EntityResponse { Id = course.Id, Name = course.Name });
		}
		return response;
	}
	/// <summary>
	/// Gets an course
	/// </summary>
	/// <param name="id">Event ID</param>
	/// <returns>Event object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<EntityResponse>> Get(int id) {
		var course = await this._context.Courses.FindAsync(id);
		return course is not null
			? this.Ok(new EntityResponse { Id = course.Id, Name = course.Name })
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
	public async Task<ActionResult<Career>> Post(Career career, int campusSingularId) {
		var existing = await this._context.Careers.FirstOrDefaultAsync(c => c.Name == career.Name);
		if (existing is not null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
		}
		var campusSingular = await this._context.CampusSingulars.FindAsync(campusSingularId);
		if (campusSingular is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		career.CampusSingular = campusSingular;
		_ = this._context.Careers.Add(career);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(career);
	}
}
