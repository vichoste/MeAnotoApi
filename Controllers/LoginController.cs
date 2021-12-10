using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using MeAnotoApi.Contexts;
using MeAnotoApi.Information;
using MeAnotoApi.Models.Users;
using MeAnotoApi.Strings;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MeAnotoApi.Controllers;
/// <summary>
/// Controller for login
/// </summary>
[ApiController, EnableCors("FrontendCors"), Route(Routes.Api + "/" + Routes.Login)]
public class LoginController : ControllerBase {
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IConfiguration _configuration;
	/// <summary>
	/// Creates the controller
	/// </summary>
	/// <param name="userManager">User manager</param>
	/// <param name="roleManager">Role manager</param>
	/// <param name="configuration">Configuration</param>
	/// <param name="context">Database context</param>
	public LoginController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, MeAnotoContext context) {
		this._userManager = userManager;
		this._roleManager = roleManager;
		this._configuration = configuration;
	}
	/// <summary>
	/// Logs in a user
	/// </summary>
	/// <param name="model">Input form</param>
	/// <returns>Token information in JSON format</returns>
	[HttpPost]
	public async Task<ActionResult<Token>> GetLoginToken([FromBody] LoginModel model) {
		try {
			var user = await this._userManager.FindByNameAsync(model.Email);
			if (user != null && await this._userManager.CheckPasswordAsync(user, model.Password)) {
				var userRoles = await this._userManager.GetRolesAsync(user);
				var authClaims = new List<Claim> {
				new(ClaimTypes.Name, user.UserName),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};
				foreach (var userRole in userRoles) {
					authClaims.Add(new(ClaimTypes.Role, userRole));
				}
				var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["JWT:Secret"]));
				var token = new JwtSecurityToken(
					issuer: this._configuration["JWT:ValidIssuer"],
					audience: this._configuration["JWT:ValidAudience"],
					expires: DateTime.Now.AddHours(1),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);
				return this.Ok(new Token {
					Status = Statuses.Ok,
					Info = new JwtSecurityTokenHandler().WriteToken(token),
					Expiration = token.ValidTo,
					Roles = userRoles
				});
			}
			return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
		} catch (Exception) {
			return this.BadRequest(new Response { Status = Statuses.InvalidOperationError, Message = Messages.InvalidOperationError });
		}
	}
}
