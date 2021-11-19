using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MeAnotoApi.Controllers;

[Route(Routes.Api + "/" + Routes.Authentication)]
[ApiController]
public class AuthenticationController : ControllerBase {
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IConfiguration _configuration;
	public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) {
		this._userManager = userManager;
		this._roleManager = roleManager;
		this._configuration = configuration;
	}
	[HttpPost]
	[Route(Routes.Login)]
	public async Task<IActionResult> Login([FromBody] LoginModel model) {
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
		return this.StatusCode(403, new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
	}
	[HttpPost]
	[Route(Routes.Register + "/" + UserRoles.Administrator)]
	public async Task<IActionResult> RegisterAdministrator([FromBody] RegisterModel model) {
		var userExists = await this._userManager.FindByNameAsync(model.Email);
		if (userExists != null) {
			return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
		}
		var user = new ApplicationUser() {
			UserName = model.Email,
			Email = model.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
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
	}
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost]
	[Route(Routes.Register + "/" + UserRoles.Manager)]
	public async Task<IActionResult> RegisterManager([FromBody] RegisterModel model) {
		var userExists = await this._userManager.FindByNameAsync(model.Email);
		if (userExists != null) {
			return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
		}
		var user = new ApplicationUser() {
			UserName = model.Email,
			Email = model.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
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
	}
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost]
	[Route(Routes.Register + "/" + UserRoles.Professor)]
	public async Task<IActionResult> RegisterProfessor([FromBody] RegisterModel model) {
		var userExists = await this._userManager.FindByNameAsync(model.Email);
		if (userExists != null) {
			return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
		}
		var user = new ApplicationUser() {
			UserName = model.Email,
			Email = model.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
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
	}
	[Authorize(Roles = UserRoles.Administrator)]
	[HttpPost]
	[Route(Routes.Register + "/" + UserRoles.Attendee)]
	public async Task<IActionResult> RegisterAttendee([FromBody] RegisterModel model) {
		var userExists = await this._userManager.FindByNameAsync(model.Email);
		if (userExists != null) {
			return this.Unauthorized(new Response { Status = Statuses.Unauthorized, Message = Messages.AuthorizationError });
		}
		var user = new ApplicationUser() {
			UserName = model.Email,
			Email = model.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
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
	}
}
