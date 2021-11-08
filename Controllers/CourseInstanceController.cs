using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers {
	[ApiController]
	[EnableCors("FrontendCors")]
	[Route(Routes.Api + "/" + Entities.CourseInstance)]
	public class CourseInstanceController : ControllerBase {
		private readonly MeAnotoContext _Context;
		public CourseInstanceController(MeAnotoContext context) => this._Context = context;
		[HttpGet(Routes.All)]
		public async Task<ActionResult<IEnumerable<CourseInstance>>> Get() => await this._Context.CourseInstances.ToListAsync();
		[HttpGet("{id}")]
		public async Task<ActionResult<CourseInstance>> Get(int id) {
			var entity = await this._Context.CourseInstances.FindAsync(id);
			return entity is not null ? this.Ok(entity) : this.NotFound(new Response { Status = Statuses.NotFound, Message = Messages.NotFoundError });
		}
		[Authorize(Roles = UserRoles.Administrator)]
		[HttpPost("{" + Entities.Course + "}")]
		public async Task<ActionResult<CourseInstance>> Post(CourseInstance entity, int courseId) {
			var course = await this._Context.Courses.FindAsync(courseId);
			if (course is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			entity.Course = course;
			_ = this._Context.CourseInstances.Add(entity);
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
		}
	}
}