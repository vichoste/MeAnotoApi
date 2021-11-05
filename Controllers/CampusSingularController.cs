﻿using System.Collections.Generic;
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
	[Route("Api/Sede")]
	public class CampusSingularController : ControllerBase {
		private readonly MeAnotoContext _Context;
		public CampusSingularController(MeAnotoContext context) => this._Context = context;
		[HttpGet("Todos")]
		public async Task<ActionResult<IEnumerable<CampusSingular>>> Get() => await this._Context.CampusSingulars.ToListAsync();
		[HttpGet("{id}")]
		public async Task<ActionResult<CampusSingular>> Get(int id) {
			var entity = await this._Context.CampusSingulars.FindAsync(id);
			return entity is not null ? this.Ok(entity) : this.BadRequest();
		}
		[HttpPost]
		public async Task<ActionResult<CampusSingular>> Post(CampusSingular entity) {
			_ = this._Context.CampusSingulars.Add(entity);
			_ = await this._Context.SaveChangesAsync();
			return this.Ok(new Response { Status = "Ok", Message = "Campus creado con éxito" });
		}
	}
}