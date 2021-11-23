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
/// Controller for course
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.Course)]
public class CourseController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public CourseController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the courses
	/// </summary>
	/// <returns>List of courses in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<Course>>> Get() => await this._context.Courses.ToListAsync();
	/// <summary>
	/// Gets a course
	/// </summary>
	/// <param name="id">Course ID</param>
	/// <returns>Course object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<Course>> Get(int id) {
		var entity = await this._context.Courses.FindAsync(id);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates a course
	/// </summary>
	/// <param name="entity">Course</param>
	/// <param name="careerId">Career ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{careerId}")]
	public async Task<ActionResult<Course>> Post(Course entity, int careerId) {
		var career = await this._context.Careers.FindAsync(careerId);
		if (career is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.Career = career;
		_ = this._context.Courses.Add(entity);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
