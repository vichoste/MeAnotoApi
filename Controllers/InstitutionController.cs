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
	[Route("Api/Institution")]
	public class InstitutionController : ControllerBase {
		private readonly MeAnotoContext _Context;
		public InstitutionController(MeAnotoContext context) => this._Context = context;
		[HttpGet("All")]
		public async Task<ActionResult<IEnumerable<Institution>>> Get() => await this._Context.Institutions.ToListAsync();
		[HttpGet("{id}")]
		public async Task<ActionResult<Institution>> Get(int id) {
			var entity = await this._Context.Institutions.FindAsync(id);
			return entity is not null ? this.Ok(entity) : this.BadRequest();
		}
		[HttpPost]
		public async Task<ActionResult<Institution>> Post(Institution entity) {
			_ = this._Context.Institutions.Add(entity);
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = "Ok", Message = "Created successfully" }); ;
		}
	}
}
