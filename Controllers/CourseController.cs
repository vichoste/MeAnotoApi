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
		var course = await this._context.Courses.FindAsync(id);
		return course is not null
			? this.Ok(course)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates a course
	/// </summary>
	/// <param name="course">Course</param>
	/// <param name="careerId">Career ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{careerId}")]
	public async Task<ActionResult<Response>> Post(Course course, int careerId) {
		var career = await this._context.Careers.FindAsync(careerId);
		if (career is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		course.Career = career;
		_ = this._context.Courses.Add(course);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
}
