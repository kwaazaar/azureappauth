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
                if (!String.IsNullOrWhiteSpace(_config.AzureCliConnString) && await TryGetToken(cancellationToken, _config.Resource, _config.AzureCliConnString, _config.TestUrlGet))
                {
                    _logger.LogInformation("Successfully obtained an accesstoken using AzureCLI");
                }
                if (!String.IsNullOrWhiteSpace(_config.ClientCredentialsConnString) && await TryGetToken(cancellationToken, _config.Resource, _config.ClientCredentialsConnString, _config.TestUrlGet))
                {
                    _logger.LogInformation("Successfully obtained an accesstoken using client credentials");
                }
                if (!String.IsNullOrWhiteSpace(_config.MSIConnString) && await TryGetToken(cancellationToken, _config.Resource, _config.MSIConnString, _config.TestUrlGet))
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

        private async Task<bool> TryGetToken(CancellationToken cancellationToken, string resource, string connString, string testUrlGet)
        {
            var provider = new AzureServiceTokenProvider(connString);

            try
            {
                var authResult = await provider.GetAuthenticationResultAsync(resource, cancellationToken: cancellationToken);
                if (authResult != null)
                {
                    _logger.LogInformation("Successfully obtained accesstoken for connString {connString}", connString);
                    _logger.LogInformation("Accesstoken: " + authResult.AccessToken);
                    if (!String.IsNullOrWhiteSpace(testUrlGet))
                    {
                        using (var httpClient = new HttpClient())
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authResult.TokenType ?? "Bearer", authResult.AccessToken);
                            using (var response = await httpClient.GetAsync(testUrlGet, cancellationToken))
                            {
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
                    return true;
                }
                _logger.LogError("Failed to get accesstoken for {resource} using connectionstring '{connString}': no result", resource, connString);)
                return false;
            }
            catch (AzureServiceTokenProviderException ex)
            {
                _logger.LogError(ex, "Failed to get accesstoken for {resource} using connectionstring '{connString}': {message}", resource, connString, ex.Message);
                return false;
            }
        }
    }
}