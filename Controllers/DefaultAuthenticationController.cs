using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using MeAnotoApi.Authentication;
using MeAnotoApi.Models.Users.Default;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MeAnotoApi.Controllers {
	[Route("Api/Authentication")]
	[ApiController]
	public class DefaultAuthenticationController : ControllerBase {
		private readonly UserManager<DefaultUser> _UserManager;
		private readonly RoleManager<IdentityRole> _RoleManager;
		private readonly IConfiguration _Configuration;
		public DefaultAuthenticationController(UserManager<DefaultUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) {
			this._UserManager = userManager;
			this._RoleManager = roleManager;
			this._Configuration = configuration;
		}
		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model) {
			var user = await this._UserManager.FindByNameAsync(model.Email);
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
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] DefaultRegisterModel model) {
			var userExists = await this._UserManager.FindByNameAsync(model.Email);
			if (userExists != null) {
				return this.Unauthorized();
			}
			DefaultUser user = model.Role switch {
				"Attendee" => new AttendeeUser() {
					Email = model.Email,
					SecurityStamp = Guid.NewGuid().ToString(),
				},
				"Professor" => new ProfessorUser() {
					Email = model.Email,
					SecurityStamp = Guid.NewGuid().ToString(),
				},
				_ => null,
			};
			if (user is not null) {
				var result = await this._UserManager.CreateAsync(user, model.Password);
				return !result.Succeeded
					? this.BadRequest(this.BadRequest(new Response { Status = "Error", Message = "Creation failed" }))
					: this.Ok(new Response { Status = "Ok", Message = "Created successfully" });
			}
			return this.Unauthorized();
		}
	}
}