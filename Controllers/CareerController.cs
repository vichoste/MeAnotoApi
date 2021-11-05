using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeAnotoApi.Controllers {
	[Authorize]
	[ApiController]
	[EnableCors("FrontendCors")]
	[Route("Api/Carrera")]
	public class CareerController : ControllerBase {
		private readonly MeAnotoContext _Context;
		public CareerController(MeAnotoContext context) => this._Context = context;
		[HttpGet("Todos")]
		public async Task<ActionResult<IEnumerable<Career>>> Get() => await this._Context.Careers.ToListAsync();
		[HttpGet("{id}")]
		public async Task<ActionResult<Career>> Get(int id) {
			var entity = await this._Context.Careers.FindAsync(id);
			return entity is not null ? this.Ok(entity) : this.BadRequest();
		}
		[HttpGet("{id}/Módulos")]
		public async Task<ActionResult<IQueryable<Career>>> GetCareers(int id) {
			var main = await this._Context.Careers.FindAsync(id);
			return main is not null ? this.Ok(from entity in this._Context.Courses where entity.Career.Id == id select entity) : this.BadRequest();
		}
		[HttpPost]
		public async Task<ActionResult<Career>> Post(Career entity) {
			_ = this._Context.Careers.Add(entity);
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = "Ok", Message = "Carrera creada con éxito" });
		}
	}
}