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

namespace MeAnotoApi.Controllers {
	[Route("Api/Authentication")]
	[ApiController]
	public class AuthenticationController : ControllerBase {
		private readonly UserManager<ApplicationUser> _UserManager;
		private readonly RoleManager<IdentityRole> _RoleManager;
		private readonly IConfiguration _Configuration;
		public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) {
			this._UserManager = userManager;
			this._RoleManager = roleManager;
			this._Configuration = configuration;
		}
		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model) {
			var user = await this._UserManager.FindByNameAsync(model.UserName);
			if (user != null && await this._UserManager.CheckPasswordAsync(user, model.Password)) {
				var userRoles = await this._UserManager.GetRolesAsync(user);
				var authClaims = new List<Claim> {
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};
				foreach (var userRole in userRoles) {
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}
				var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._Configuration["JWT:Secret"]));
				var token = new JwtSecurityToken(
					issuer: this._Configuration["JWT:ValidIssuer"],
					audience: this._Configuration["JWT:ValidAudience"],
					expires: DateTime.Now.AddHours(1),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
					);
				return this.Ok(new {
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo
				});
			}
			return this.Unauthorized();
		}
		[HttpPost]
		[Route("Register/Administrator")]
		public async Task<IActionResult> RegisterAdministrator([FromBody] RegisterModel model) {
			var userExists = await this._UserManager.FindByNameAsync(model.UserName);
			if (userExists != null) {
				return this.Unauthorized();
			}
			var user = new ApplicationUser() {
				UserName = model.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
			};
			var result = await this._UserManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) {
				return this.BadRequest(this.BadRequest(new Response { Status = "Error", Message = "Creation failed" }));
			}
			if (!await this._RoleManager.RoleExistsAsync(UserRoles.Administrator)) {
				_ = await this._RoleManager.CreateAsync(new IdentityRole(UserRoles.Administrator));
			}
			if (await this._RoleManager.RoleExistsAsync(UserRoles.Administrator)) {
				_ = await this._UserManager.AddToRoleAsync(user, UserRoles.Administrator);
			}
			return this.Ok(new Response { Status = "Ok", Message = "Created successfully" });
		}
		[Authorize(Roles = UserRoles.Administrator)]
		[HttpPost]
		[Route("Register/Manager")]
		public async Task<IActionResult> RegisterManager([FromBody] RegisterModel model) {
			var userExists = await this._UserManager.FindByNameAsync(model.UserName);
			if (userExists != null) {
				return this.Unauthorized();
			}
			var user = new ApplicationUser() {
				UserName = model.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
			};
			var result = await this._UserManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) {
				return this.BadRequest(this.BadRequest(new Response { Status = "Error", Message = "Creation failed" }));
			}
			if (!await this._RoleManager.RoleExistsAsync(UserRoles.Manager)) {
				_ = await this._RoleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
			}
			if (await this._RoleManager.RoleExistsAsync(UserRoles.Manager)) {
				_ = await this._UserManager.AddToRoleAsync(user, UserRoles.Manager);
			}
			return this.Ok(new Response { Status = "Ok", Message = "Created successfully" });
		}
		[Authorize(Roles = UserRoles.Administrator)]
		[HttpPost]
		[Route("Register/Professor")]
		public async Task<IActionResult> RegisterProfessor([FromBody] RegisterModel model) {
			var userExists = await this._UserManager.FindByNameAsync(model.UserName);
			if (userExists != null) {
				return this.Unauthorized();
			}
			var user = new ApplicationUser() {
				UserName = model.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
			};
			var result = await this._UserManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) {
				return this.BadRequest(this.BadRequest(new Response { Status = "Error", Message = "Creation failed" }));
			}
			if (!await this._RoleManager.RoleExistsAsync(UserRoles.Professor)) {
				_ = await this._RoleManager.CreateAsync(new IdentityRole(UserRoles.Professor));
			}
			if (await this._RoleManager.RoleExistsAsync(UserRoles.Professor)) {
				_ = await this._UserManager.AddToRoleAsync(user, UserRoles.Professor);
			}
			return this.Ok(new Response { Status = "Ok", Message = "Created successfully" });
		}
		[Authorize(Roles = UserRoles.Administrator)]
		[HttpPost]
		[Route("Register/Attendee")]
		public async Task<IActionResult> RegisterAttendee([FromBody] RegisterModel model) {
			var userExists = await this._UserManager.FindByNameAsync(model.UserName);
			if (userExists != null) {
				return this.Unauthorized();
			}
			var user = new ApplicationUser() {
				UserName = model.UserName,
				SecurityStamp = Guid.NewGuid().ToString(),
			};
			var result = await this._UserManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) {
				return this.BadRequest(this.BadRequest(new Response { Status = "Error", Message = "Creation failed" }));
			}
			if (!await this._RoleManager.RoleExistsAsync(UserRoles.Attendee)) {
				_ = await this._RoleManager.CreateAsync(new IdentityRole(UserRoles.Attendee));
			}
			if (await this._RoleManager.RoleExistsAsync(UserRoles.Attendee)) {
				_ = await this._UserManager.AddToRoleAsync(user, UserRoles.Attendee);
			}
			return this.Ok(new Response { Status = "Ok", Message = "Created successfully" });
		}
	}
}