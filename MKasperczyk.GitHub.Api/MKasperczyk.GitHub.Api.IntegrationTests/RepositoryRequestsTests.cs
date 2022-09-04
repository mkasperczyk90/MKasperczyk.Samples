using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MKasperczyk.GitHub.Api.IntegrationTests.Setup;
using MKasperczyk.GitHub.Api.Services.Contract;
using MKasperczyk.GitHub.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MKasperczyk.GitHub.Api.Models;
using MKasperczyk.GitHub.Api.IntegrationTests.Mocks;
using MKasperczyk.GitHub.Api.IntegrationTests.Helpers;

namespace MKasperczyk.GitHub.Api.IntegrationTests
{
    public class RepositoryRequestsTests : IClassFixture<GitHubApplicationFactory<Program>>
    {
        private readonly GitHubApplicationFactory<Program> _factory;
        public RepositoryRequestsTests(GitHubApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/repositoryWithDetails/notExistingTestOwner")]
        [InlineData("/repository/notExistingTestOwner")]
        public async Task RepositoryEndpointsShouldBeSucessfully(string url)
        {
            // given
            var client = GetClient();

            // when
            using var httpResponseMessage = await client.GetAsync(url);

            // then
            httpResponseMessage.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task RepositoryGetEndpointShouldReturnCorrectValues()
        {
            // given
            string owner = "testOwner";
            var client = GetClient();

            // when
            using var httpResponseMessage = await client.GetAsync($"/repository/{owner}");

            // then
            httpResponseMessage.EnsureSuccessStatusCode();
            var result = await httpResponseMessage.Content.ReadAsStringAsync();
        }

        [Fact]
        public async Task RepositoryWithDetailsGetEndpointShouldReturnCorrectValues()
        {
            // given
            string owner = "testOwner";
            var client = GetClient();

            // when
            using var httpResponseMessage = await client.GetAsync($"/repositoryWithDetails/{owner}");

            // then
            httpResponseMessage.EnsureSuccessStatusCode();
            var result = await httpResponseMessage.Content.ReadAsStringAsync();
        }

        private HttpClient GetClient()
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IGitHubService>((serviceProvider) =>
                    {
                        var mockData = GitHubApiMock.Get();
                        var mockHandler = HttpClientHelper
                        .GetResults(mockData);

                        var mockHttpClient = new HttpClient(mockHandler.Object);
                        mockHttpClient.BaseAddress = new Uri("https://127.0.0.1");

                        return new GitHubService(mockHttpClient, Mock.Of<ILogger<GitHubService>>());
                    });
                });
            }).CreateClient();
        }
    }
}
