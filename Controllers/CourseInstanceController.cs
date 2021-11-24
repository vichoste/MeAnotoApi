using System.Collections.Generic;
using System.Linq;
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
	/// Gets all the course instances owned by the current user
	/// </summary>
	/// <returns>List of owned courses in JSON format</returns>
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<CourseInstance>>> Get() {
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var courseInstances = await this._context.CourseInstances.ToListAsync();
		var myCourseInstances = courseInstances.Where(
			c => c.Professors.Any(
			p => p == professor
		));
		return this.Ok(myCourseInstances);
	}
	/// <summary>
	/// Gets a course instance
	/// </summary>
	/// <param name="id">Course instance ID</param>
	/// <returns>Course instance object in JSON format</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<CourseInstance>> Get(int id) {
		var courseInstance = await this._context.CourseInstances.FindAsync(id);
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		return professor is null
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: !courseInstance.Professors.Any(p => p == professor)
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: courseInstance is not null ? this.Ok(courseInstance)
			: this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	/// <summary>
	/// Creates a course instance
	/// </summary>
	/// <param name="courseInstance">Course instance</param>
	/// <param name="courseId">Course ID</param>
	/// <returns>OK if sucessfully in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor)]
	[HttpPost("{courseId}")]
	public async Task<ActionResult<CourseInstance>> Post(CourseInstance courseInstance, int courseId) {
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var course = await this._context.Courses.FindAsync(courseId);
		if (course is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		courseInstance.Course = course;
		courseInstance.Professors.Add(professor);
		_ = this._context.CourseInstances.Add(courseInstance);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
	}
	/// <summary>
	/// Enrolls a professor into a course instance
	/// </summary>
	/// <param name="id">Course instance ID</param>
	/// <returns>OK if enrolled successfully</returns>
	[Authorize(Roles = UserRoles.Professor)]
	[HttpPost("{id}/" + Routes.Enroll + "/" + UserRoles.Professor)]
	public async Task<ActionResult<CourseInstance>> EnrollProfessor(int id) {
		var name = this.HttpContext.User.Identity.Name;
		var professor = this._context.Professors.First(p => p.UserName == name);
		if (professor is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var courseInstance = await this._context.CourseInstances.FindAsync(id);
		if (courseInstance is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		if (!courseInstance.Professors.Any(p => p == professor)) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		courseInstance.Professors.Add(professor);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.EnrolledOk });
	}
	/// <summary>
	/// Enrolls an professor into a course instance
	/// </summary>
	/// <param name="id">Course instance ID</param>
	/// <returns>OK if enrolled successfully</returns>
	[Authorize(Roles = UserRoles.Attendee)]
	[HttpPost("{id}/" + Routes.Enroll + "/" + UserRoles.Attendee)]
	public async Task<ActionResult<CourseInstance>> EnrollAttendee(int id) {
		var name = this.HttpContext.User.Identity.Name;
		var attendee = this._context.Attendees.First(p => p.UserName == name);
		if (attendee is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var courseInstance = await this._context.CourseInstances.FindAsync(id);
		if (courseInstance is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		courseInstance.Attendees.Add(attendee);
		_ = await this._context.SaveChangesAsync();
		return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.EnrolledOk });
	}
	/// <summary>
	/// Gets the amount of attendees in a course instance
	/// </summary>
	/// <param name="id">Course instance ID</param>
	/// <returns>Attendee list in JSON format</returns>
	[Authorize(Roles = UserRoles.Professor)]
	[HttpGet("{id}/" + UserRoles.Attendee + "/" + Routes.Count)]
	public async Task<ActionResult<CourseInstance>> GetAttendeeCount(int id) {
		var name = this.HttpContext.User.Identity.Name;
		var courseInstance = await this._context.CourseInstances.FindAsync(id);
		var professor = this._context.Professors.First(p => p.UserName == name);
		return professor is null
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: !courseInstance.Professors.Any(p => p == professor)
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: !courseInstance.Professors.Any(p => p.Email == name)
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: courseInstance.Attendees is null || courseInstance.Attendees.Count is 0
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: this.Ok(new Response { Status = Statuses.Ok, Message = courseInstance.Attendees.Count.ToString() });
	}
}
