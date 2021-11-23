using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MeAnotoApi;
/// <summary>
/// Program
/// </summary>
public class Program {
	/// <summary>
	/// Main
	/// </summary>
	/// <param name="args">Parameters</param>
	public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();
	/// <summary>
	/// Create host builder
	/// </summary>
	/// <param name="args">Parameters</param>
	/// <returns>Builder</returns>
	public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => _ = webBuilder.UseStartup<Startup>());
}
