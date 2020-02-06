using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AppAuthTest
{
    internal class AppAuthTester : IHostedService
    {
        private readonly IHostApplicationLifetime _lifetimeManager;
        private readonly ILogger<AppAuthTester> _logger;
        private readonly AppAuthConfig _config;

        public AppAuthTester(IHostApplicationLifetime lifetimeManager, ILogger<AppAuthTester> logger, AppAuthConfig config)
        {
            this._lifetimeManager = lifetimeManager;
            this._logger = logger;
            this._config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var provider = new AzureServiceTokenProvider(_config.AzureServicesAuthConnectionString);

            try
            {
                _logger.LogInformation("Requesting accesstoken...");
                var authResult = await provider.GetAuthenticationResultAsync(_config.Resource, tenantId: _config.TenantId, cancellationToken: cancellationToken);
                if (authResult != null)
                {
                    _logger.LogInformation("Accesstoken: " + authResult.AccessToken);
                    if (!String.IsNullOrWhiteSpace(_config.TestUrlGet))
                    {
                        using var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authResult.TokenType ?? "Bearer", authResult.AccessToken);
                        using var response = await httpClient.GetAsync(_config.TestUrlGet, cancellationToken);
                        var responseContent = await response.Content?.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            _logger.LogError("Failed to invoke test url. Statuscode: {statusCode}, message: {message}", response.StatusCode, responseContent);
                        }
                        else
                        {
                            _logger.LogInformation("Successfully invoked test url. Statuscode: {statusCode}, message: {message}", response.StatusCode, responseContent);
                        }
                    }
                }
            }
            catch (AzureServiceTokenProviderException ex)
            {
                _logger.LogError(ex, "Failed to get accesstoken: {message}", ex.Message);
                throw;
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
   }
}