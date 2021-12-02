
using System.Collections.Generic;

using MeAnotoApi.Authentication;
using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for account
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route(Routes.Api + "/" + Routes.Account)]
[ApiController]
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
	/// Gets all the users
	/// </summary>
	/// <returns>Users in JSON format</returns>
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpGet]
	[Route(Routes.All)]
	public ActionResult<IEnumerable<ApplicationUser>> GetApplicationUsers() => this.Ok(this._userManager.Users);
}
