using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Contexts;
using MeAnotoApi.Information;
using MeAnotoApi.Models.Users;
using MeAnotoApi.Strings;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for account
/// </summary>
[Authorize(Roles = UserRoles.Administrator), ApiController, EnableCors("FrontendCors"), Route(Routes.Api + "/" + Routes.Account)]
public class AccountController : ControllerBase {
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
	public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, MeAnotoContext context) {
		this._userManager = userManager;
		this._roleManager = roleManager;
		this._configuration = configuration;
		this._context = context;
	}
	/// <summary>
	/// Deletes a user
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <returns>OK if deleted successfully in JSON format</returns>
	[HttpDelete("{userId}")]
	public async Task<ActionResult<Response>> DeleteApplicationUser(string userId) {
		try { // TODO Cascade?
			var user = await this._userManager.Users.FirstAsync(u => u.Id == userId);
			IdentityResult result;
			result = await this._userManager.DeleteAsync(user);
			return !result.Succeeded
				? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
				: this.Ok(new Response { Status = Statuses.Ok, Message = Messages.DeleteOk });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Gets all the users
	/// </summary>
	/// <returns>Users in JSON format</returns>
	[HttpGet(Routes.All)]
	public ActionResult<IEnumerable<ApplicationUser>> ListApplicationUsers() => this.Ok(this._userManager.Users);
	/// <summary>
	/// Gets a single user
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <returns>User in JSON format</returns>
	[HttpGet("{userId}")]
	public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string userId) {
		try {
			var user = await this._userManager.Users.FirstAsync(u => u.Id == userId);
			return user is null
				? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
				: this.Ok(user);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Creates an administrator
	/// </summary>
	/// <param name="model">Input form</param>
	/// <returns>OK if successful in JSON format</returns>
	[AllowAnonymous, HttpPost(Routes.Register + "/" + UserRoles.Administrator)]
	public async Task<ActionResult<Token>> RegisterAdministrator([FromBody] RegisterModel model) { // TODO this thing is a vulnerability
		try {
			var userExists = await this._userManager.FindByNameAsync(model.Email);
			if (userExists != null) {
				return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
			}
			var user = new ApplicationUser() {
				UserName = model.Email,
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				Run = model.Run,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};
			var result = await this._userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) {
				return this.StatusCode(500, new Response { Status = Statuses.InternalServerError, Message = Messages.InternalServerError });
			}
			if (!await this._roleManager.RoleExistsAsync(UserRoles.Administrator)) {
				_ = await this._roleManager.CreateAsync(new(UserRoles.Administrator));
			}
			if (await this._roleManager.RoleExistsAsync(UserRoles.Administrator)) {
				_ = await this._userManager.AddToRoleAsync(user, UserRoles.Administrator);
			}
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Creates a manager
	/// </summary>
	/// <param name="model">Input form</param>
	/// <param name="institutionId">Institution ID</param>
	/// <returns>OK if successful in JSON format</returns>
	[HttpPost(Routes.Register + "/" + UserRoles.Manager + "/{institutionId}")]
	public async Task<ActionResult<Token>> RegisterManager([FromBody] RegisterModel model, int institutionId) {
		try {
			var institution = await this._context.Institutions.FindAsync(institutionId);
			if (institution is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var userExists = await this._userManager.FindByNameAsync(model.Email);
			if (userExists != null) {
				return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
			}
			var user = new ApplicationUser() {
				UserName = model.Email,
				Email = model.Email,
				Institution = institution,
				SecurityStamp = Guid.NewGuid().ToString(),
				Run = model.Run,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};
			var result = await this._userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) {
				return this.StatusCode(500, new Response { Status = Statuses.InternalServerError, Message = Messages.InternalServerError });
			}
			if (!await this._roleManager.RoleExistsAsync(UserRoles.Manager)) {
				_ = await this._roleManager.CreateAsync(new(UserRoles.Manager));
			}
			if (await this._roleManager.RoleExistsAsync(UserRoles.Manager)) {
				_ = await this._userManager.AddToRoleAsync(user, UserRoles.Manager);
			}
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Creates a professor
	/// </summary>
	/// <param name="model">Input form</param>
	/// <param name="institutionId">Institution ID</param>
	/// <returns>OK if successful in JSON format</returns>
	[HttpPost(Routes.Register + "/" + UserRoles.Professor + "/{institutionId}")]
	public async Task<ActionResult<Token>> RegisterProfessor([FromBody] RegisterModel model, int institutionId) {
		try {
			var institution = await this._context.Institutions.FindAsync(institutionId);
			if (institution is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var userExists = await this._userManager.FindByNameAsync(model.Email);
			if (userExists != null) {
				return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
			}
			var user = new Professor() {
				UserName = model.Email,
				Email = model.Email,
				Institution = institution,
				SecurityStamp = Guid.NewGuid().ToString(),
				Run = model.Run,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};
			var result = await this._userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) {
				return this.StatusCode(500, new Response { Status = Statuses.InternalServerError, Message = Messages.InternalServerError });
			}
			if (!await this._roleManager.RoleExistsAsync(UserRoles.Professor)) {
				_ = await this._roleManager.CreateAsync(new(UserRoles.Professor));
			}
			if (await this._roleManager.RoleExistsAsync(UserRoles.Professor)) {
				_ = await this._userManager.AddToRoleAsync(user, UserRoles.Professor);
			}
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
	/// <summary>
	/// Creates an attendee
	/// </summary>
	/// <param name="model">Input form</param>
	/// <param name="institutionId">Institution ID</param>
	/// <returns>OK if successful in JSON format</returns>
	[HttpPost(Routes.Register + "/" + UserRoles.Attendee + "/{institutionId}")]
	public async Task<ActionResult<Token>> RegisterAttendee([FromBody] RegisterModel model, int institutionId) {
		try {
			var institution = await this._context.Institutions.FindAsync(institutionId);
			if (institution is null) {
				return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError });
			}
			var userExists = await this._userManager.FindByNameAsync(model.Email);
			if (userExists != null) {
				return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
			}
			var user = new Attendee() {
				UserName = model.Email,
				Email = model.Email,
				Institution = institution,
				SecurityStamp = Guid.NewGuid().ToString(),
				Run = model.Run,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};
			var result = await this._userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) {
				return this.StatusCode(500, new Response { Status = Statuses.InternalServerError, Message = Messages.InternalServerError });
			}
			if (!await this._roleManager.RoleExistsAsync(UserRoles.Attendee)) {
				_ = await this._roleManager.CreateAsync(new(UserRoles.Attendee));
			}
			if (await this._roleManager.RoleExistsAsync(UserRoles.Attendee)) {
				_ = await this._userManager.AddToRoleAsync(user, UserRoles.Attendee);
			}
			return this.Ok(new Response { Status = Statuses.Ok, Message = Messages.CreatedOk });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
}
