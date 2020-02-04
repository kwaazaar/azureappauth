using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppAuthTest
{
    internal class AppAuthTester : IHostedService
    {
        private readonly IApplicationLifetime _lifetimeManager;
        private readonly ILogger<AppAuthTester> _logger;
        private readonly AppAuthConfig _config;

        public AppAuthTester(IApplicationLifetime lifetimeManager, ILogger<AppAuthTester> logger, AppAuthConfig config)
        {
            this._lifetimeManager = lifetimeManager;
            this._logger = logger;
            this._config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to get accesstokens using the AzureServiceTokenProvider");

            try
            {
                if (await TryGetToken(cancellationToken, _config.Resource, _config.AzureCliConnString))
                {
                    _logger.LogInformation("Successfully obtained an accesstoken using AzureCLI");
                }
                if (await TryGetToken(cancellationToken, _config.Resource, _config.ClientCredentialsConnString))
                {
                    _logger.LogInformation("Successfully obtained an accesstoken using client credentials");
                }
                if (await TryGetToken(cancellationToken, _config.Resource, _config.MSIConnString))
                {
                    _logger.LogInformation("Successfully obtained an accesstoken using Managed Service Identity");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {message}", ex.Message);
                throw;
            }

            _lifetimeManager.StopApplication();

            _logger.LogInformation("Done.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task<bool> TryGetToken(CancellationToken cancellationToken, string resource, string connString)
        {
            var provider = new AzureServiceTokenProvider(connString);

            try
            {
                var token = await provider.GetAccessTokenAsync(resource, cancellationToken: cancellationToken);
                return true;
            }
            catch (AzureServiceTokenProviderException ex)
            {
                _logger.LogError(ex, "Failed to get token using connectionstring '{connString}': {message}", connString, ex.Message);
                return false;
            }
        }
    }
}