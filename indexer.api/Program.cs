using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace indexer.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(opts => 
                    {
                        opts.ClearProviders();
                        opts.SetMinimumLevel(LogLevel.Trace);
                        opts.AddConsole();
                    })
                    .ConfigureAppConfiguration((context, opts) => opts.AddEnvironmentVariables("ASPNETCORE_"));
                });
    }
}
