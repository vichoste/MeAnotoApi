using System.Collections.Generic;
using System.Text;

using MeAnotoApi.Contexts;
using MeAnotoApi.Models.Users;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace MeAnotoApi {
	public class Startup {
		public Startup(IConfiguration configuration) => this.Configuration = configuration;
		public IConfiguration Configuration { get; }
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			_ = services.AddControllers();
			_ = services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeAnotoApi", Version = "v1" });
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
					Description = @"Format: Bearer [yourtokenhere]",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement() { {
					new OpenApiSecurityScheme {
						Reference = new OpenApiReference {
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						},
						Scheme = "oauth2",
						Name = "Bearer",
						In = ParameterLocation.Header
					}, new List<string>()
				}});
			});
			var connection = this.Configuration.GetConnectionString("Database");
			_ = services.AddDbContext<MeAnotoContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
			_ = services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MeAnotoContext>().AddDefaultTokenProviders();
			_ = services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options => {
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters() {
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidAudience = this.Configuration["JWT:ValidAudience"],
					ValidIssuer = this.Configuration["JWT:ValidIssuer"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["JWT:Secret"]))
				};
			});
			_ = services.AddCors(options => options.AddPolicy("FrontendCors", builder => _ = builder.WithOrigins("http://localhost:8080", "http://127.0.0.1:8080").AllowAnyHeader().AllowAnyMethod()));
		}
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				_ = app.UseDeveloperExceptionPage();
				_ = app.UseSwagger();
				_ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeAnotoApi v1"));
			}
			_ = app.UseRouting();
			_ = app.UseCors("FrontendCors");
			_ = app.UseAuthentication();
			_ = app.UseAuthorization();
			_ = app.UseEndpoints(endpoints => _ = endpoints.MapControllers());
		}
	}
}
