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
	/// Creates a course
	/// </summary>
	/// <param name="course">Course</param>
	/// <param name="careerId">Career ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost("{careerId}")]
	public async Task<ActionResult<Course>> Post(Course course, int careerId) {
		var existing = await this._context.Courses.FirstOrDefaultAsync(c => c.Name == course.Name);
		if (existing is not null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.DuplicatedError });
		}
		var career = await this._context.Careers.FindAsync(careerId);
		if (career is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		course.Career = career;
		_ = this._context.Courses.Add(course);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(course);
	}
}
