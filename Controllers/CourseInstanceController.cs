using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers;

[ApiController]
[EnableCors("FrontendCors")]
[Route(Routes.Api + "/" + Entities.CourseInstance)]
public class CourseInstanceController : ControllerBase {
	private readonly MeAnotoContext _context;
	public CourseInstanceController(MeAnotoContext context) => this._context = context;
	[HttpGet(Routes.All)]
	public async Task<ActionResult<IEnumerable<CourseInstance>>> Get() => await this._context.CourseInstances.ToListAsync();
	[HttpGet("{id}")]
	public async Task<ActionResult<CourseInstance>> Get(int id) {
		var entity = await this._context.CourseInstances.FindAsync(id);
		return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
	}
	[Authorize(Roles = UserRoles.Administrator)]
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
	//[Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Professor)]
	//[HttpGet("{" + Entities.CourseInstance + "}" + "/{" + UserRoles.Attendee + "}/" + Routes.Count)]
	//public async Task<ActionResult<int>> GetAttendeeCount(int id) {
	//	var courseInstance = await this._context.CourseInstances.FindAsync(id);
	//	return courseInstance is null ? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError }) : this.Ok(courseInstance.Attendees.Count);
	//}
}
