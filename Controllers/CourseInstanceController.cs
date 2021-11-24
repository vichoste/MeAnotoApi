using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for course instance
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.CourseInstance)]
public class CourseInstanceController : ControllerBase {
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IConfiguration _configuration;
	private readonly MeAnotoContext _context;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="userManager">User manager</param>
	/// <param name="roleManager">Role manager</param>
	/// <param name="configuration">Configuration</param>
	/// <param name="context">Database context</param>
	public CourseInstanceController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, MeAnotoContext context) {
		this._userManager = userManager;
		this._roleManager = roleManager;
		this._configuration = configuration;
		this._context = context;
	}
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
		var userName = this.HttpContext.User.Identity.Name;
		var user = await this._context.Professors.Where(p => p.UserName == userName).ToListAsync();
		foreach (var p in user) {
			System.Diagnostics.Debug.WriteLine($"p: {p.UserName}");
		}
		System.Diagnostics.Debug.WriteLine($"User: {userName}");
		//if (user is null) {
		//	return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		//}
		//var course = await this._context.Courses.FindAsync(courseId);
		//if (course is null) {
		//	return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		//}
		//entity.Course = course;
		//entity.Professors.Add(user);
		//_ = this._context.CourseInstances.Add(entity);
		//_ = await this._context.SaveChangesAsync();
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
		var userName = this.HttpContext.User.Identity.Name;
		var user = this._context.Professors.First(p => p.Email == userName);
		if (user is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		var courseInstance = await this._context.CourseInstances.FindAsync(id);
		if (courseInstance is null) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
		}
		courseInstance.Professors.Add(user);
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
		var userName = this.HttpContext.User.Identity.Name;
		var courseInstance = await this._context.CourseInstances.FindAsync(id);
		return !courseInstance.Professors.Any(p => p.Email == userName) ? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError }) : courseInstance.Attendees is null || courseInstance.Attendees.Count is 0 ? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError }) : this.Ok(new Response { Status = Statuses.Ok, Message = courseInstance.Attendees.Count.ToString() });
	}
}
