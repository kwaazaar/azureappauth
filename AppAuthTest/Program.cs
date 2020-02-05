using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppAuthTest
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    if (args != null) { config.AddCommandLine(args); }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var config = hostContext.Configuration;

                    // Tester
                    var testerConfig = config.GetSection("AppAuthTester").Get<AppAuthConfig>();
                    services.AddSingleton(testerConfig);
                    services.AddSingleton<IHostedService, AppAuthTester>();
                })
                .ConfigureLogging((hostContext, builder) =>
                {
                    var config = hostContext.Configuration;
                    builder.AddConfiguration(config.GetSection("Logging"));
                    builder.AddConsole(c =>
                    {
                        c.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fff ";
                    });
                });

            try
            {
                await hostBuilder.RunConsoleAsync();
                return 0;
            }
            catch (Exception) { return 1; } // Ignore the exception, it has already been logged
        }
    }
}
