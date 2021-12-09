using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for account
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.Administrator)]
[Route(Routes.Api + "/" + Routes.Account)]
[ApiController]
public class AccountController : ControllerBase {
	private readonly UserManager<ApplicationUser> _userManager;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="userManager">User manager</param>
	/// <param name="roleManager">Role manager</param>
	/// <param name="configuration">Configuration</param>
	/// <param name="context">Database context</param>
	public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, MeAnotoContext context) => this._userManager = userManager;
	/// <summary>
	/// Gets all the users
	/// </summary>
	/// <returns>Users in JSON format</returns>
	[HttpGet(Routes.All)]
	public ActionResult<IEnumerable<ApplicationUser>> GetApplicationUsers() => this.Ok(this._userManager.Users);
	/// <summary>
	/// Gets a single user
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <returns>User in JSON format</returns>
	[HttpGet("{userId}")]
	public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string userId) {
		var user = await this._userManager.Users.FirstAsync(u => u.Id == userId);
		return user is null
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: this.Ok(user);
	}
	/// <summary>
	/// Deletes a user
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <returns>OK if deleted successfully in JSON format</returns>
	[HttpDelete("{userId}")]
	public async Task<ActionResult<Response>> DeleteUser(string userId) {
		var user = await this._userManager.Users.FirstAsync(u => u.Id == userId);
		IdentityResult result;
		try { // TODO Cascade?
			result = await this._userManager.DeleteAsync(user);
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.CascadeNotImplemented });
		}
		return !result.Succeeded
			? this.BadRequest(new Response { Status = Statuses.BadRequest, Message = Messages.BadRequestError })
			: this.Ok(new Response { Status = Statuses.Ok, Message = Messages.DeleteOk });
	}
}
