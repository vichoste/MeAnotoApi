using MeAnotoApi.Contexts;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MeAnotoApi {
	public class Startup {
		public Startup(IConfiguration configuration) => this.Configuration = configuration;
		public IConfiguration Configuration { get; }
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			_ = services.AddControllers();
			_ = services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeAnotoApi", Version = "v1" }));
			var connection = this.Configuration.GetConnectionString("Database");
			_ = services.AddDbContext<MeAnotoContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
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
			_ = app.UseEndpoints(endpoints => {
				_ = endpoints.MapControllers();
			});
		}
	}
}
