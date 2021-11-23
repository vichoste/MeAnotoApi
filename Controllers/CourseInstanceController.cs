using System.Collections.Generic;
using System.Security.Claims;
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
/// Controller for course instance
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.CourseInstance)]
public class CourseInstanceController : ControllerBase {
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="context">Database context</param>
	public CourseInstanceController(MeAnotoContext context) => this._context = context;
	/// <summary>
	/// Gets all the course instances
	/// </summary>
	/// <returns>List of courses in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<CourseInstance>>> Get() => await this._context.CourseInstances.ToListAsync();
	/// <summary>
	/// Gets a course instance
	/// </summary>
	/// <param name="id">Course instance ID</param>
	/// <returns>Course instance object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<CourseInstance>> Get(int id) {
		var entity = await this._context.CourseInstances.FindAsync(id);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates a course instance
	/// </summary>
	/// <param name="entity">Course instance</param>
	/// <param name="courseId">Course ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor)]
	[HttpPost("{courseId}")]
	public async Task<ActionResult<CourseInstance>> Post(CourseInstance entity, int courseId) {
		var course = await this._context.Courses.FindAsync(courseId);
		if (course is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		entity.Course = course;
		_ = this._context.CourseInstances.Add(entity);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
	[Authorize(Roles = UserRoles.Professor)]
	[HttpPost("{id}/" + Routes.Enroll + "/" + UserRoles.Professor)]
	public async Task<ActionResult<CourseInstance>> EnrollProfessor(int id) { // TODO Verify who is trying to enroll
		var courseInstance = await this._context.CourseInstances.FindAsync(id);
		if (courseInstance is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var user = this.User.FindFirst(ClaimTypes.Name).Value;
		var role = this.User.FindFirst(ClaimTypes.Role).Value;
		if (user is null || user is not null && role is not UserRoles.Professor) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.EnrolledOk });
	}
	[Authorize(Roles = UserRoles.Professor)]
	[HttpGet("{id}/" + UserRoles.Attendee + "/" + Routes.Count)]
	public async Task<ActionResult<CourseInstance>> GetAttendeeCount(int id) { // TODO Verify ownership
																			   //var userId = this.User.FindFirst(ClaimTypes.Name).Value;
																			   //if (userId is null) {
																			   //	return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
																			   //}
		var courseInstance = await this._context.CourseInstances.FindAsync(id);
		//if (!courseInstance.Professors.Any(p => p.Email == userId)) {
		//	return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		//}
		return courseInstance.Attendees is null || courseInstance.Attendees.Count is 0 ? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError }) : this.Ok(new Response { Status = Statuses.Ok, Message = courseInstance.Attendees.Count.ToString() });
	}
}
