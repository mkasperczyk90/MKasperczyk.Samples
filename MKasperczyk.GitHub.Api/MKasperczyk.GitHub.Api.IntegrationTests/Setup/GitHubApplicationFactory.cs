using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MKasperczyk.GitHub.Api.IntegrationTests.Helpers;
using MKasperczyk.GitHub.Api.IntegrationTests.Mocks;
using MKasperczyk.GitHub.Api.Models;
using MKasperczyk.GitHub.Api.Services;
using MKasperczyk.GitHub.Api.Services.Contract;
using Moq;

namespace MKasperczyk.GitHub.Api.IntegrationTests.Setup
{
    public class GitHubApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var mockData = GitHubApiMock.Get();
            var mockHandler = HttpClientHelper
            .GetResults<IEnumerable<RepositoryInfo>>(mockData);

            var mockHttpClient = new HttpClient(mockHandler.Object);
            mockHttpClient.BaseAddress = new Uri("https://127.0.0.1");

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IGitHubService>((serviceProvider) =>
                {
                    return new GitHubService(mockHttpClient, Mock.Of<ILogger<GitHubService>>());
                });
            });
        }
    }
}
