using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
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

                    // ApplicationInsights
                    AddApplicationInsights(services, config);

                    // Tester
                    var testerConfig = config.GetSection("AppAuthTester").Get<AppAuthConfig>();
                    services.AddSingleton(testerConfig);
                    services.AddSingleton<IHostedService, AppAuthTester>();
                })
                .ConfigureLogging((hostContext, builder) =>
                {
                    var config = hostContext.Configuration;
                    builder.AddConfiguration(config.GetSection("Logging"));
                    builder.AddApplicationInsights(config["ApplicationInsights:InstrumentationKey"]);
                    builder.AddConsole();
                });

            try
            {
                await hostBuilder.RunConsoleAsync();
                return 0;
            }
            catch (Exception) { return 1; } // Ignore the exception, it has already been logged
        }

        private static void AddApplicationInsights(IServiceCollection services, IConfiguration config)
        {
            //services.AddTeamNameToTelemetry("B");
            //services.AddCloudRoleNameToTelemetry("AppAuthTest");
            services.AddSingleton<TelemetryClient>();
            services.AddApplicationInsightsKubernetesEnricher();
            services.AddSingleton<ITelemetryModule, DiagnosticsTelemetryModule>();
            services.AddApplicationInsightsTelemetry(config["ApplicationInsights:InstrumentationKey"]);
            services.ConfigureTelemetryModule<DiagnosticsTelemetryModule>((module, options) =>
            {
                module.IsHeartbeatEnabled = false; // No heartbeat for jobs/console-apps
            });
        }
    }
}
