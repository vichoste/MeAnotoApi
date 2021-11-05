using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Authentication;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers {
	[Authorize]
	[ApiController]
	[EnableCors("FrontendCors")]
	[Route("Api/Módulo")]
	public class CourseController : ControllerBase {
		private readonly MeAnotoContext _Context;
		public CourseController(MeAnotoContext context) => this._Context = context;
		[HttpGet("Todos")]
		public async Task<ActionResult<IEnumerable<Course>>> Get() => await this._Context.Courses.ToListAsync();
		[HttpGet("{id}")]
		public async Task<ActionResult<Course>> Get(int id) {
			var entity = await this._Context.Courses.FindAsync(id);
			return entity is not null ? this.Ok(entity) : this.BadRequest();
		}
		[HttpPost]
		public async Task<ActionResult<Course>> Post(Course entity) {
			_ = this._Context.Courses.Add(entity);
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = "Ok", Message = "Curso creado con éxito" });
		}
	}
}