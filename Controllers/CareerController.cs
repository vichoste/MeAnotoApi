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
	[Authorize]
	[ApiController]
	[EnableCors("FrontendCors")]
	[Route("Api/Career")]
	public class CareerController : ControllerBase {
		private readonly MeAnotoContext _Context;
		public CareerController(MeAnotoContext context) => this._Context = context;
		[HttpGet("All")]
		public async Task<ActionResult<IEnumerable<Career>>> Get() => await this._Context.Careers.ToListAsync();
		[HttpGet("{id}")]
		public async Task<ActionResult<Career>> Get(int id) {
			var entity = await this._Context.Careers.FindAsync(id);
			return entity is not null ? this.Ok(entity) : this.BadRequest();
		}
		[HttpPost]
		public async Task<ActionResult<Career>> Post(Career entity) {
			_ = this._Context.Careers.Add(entity);
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = "Ok", Message = "Created successfully" });
		}
	}
}