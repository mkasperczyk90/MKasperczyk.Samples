using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using MKasperczyk.GitHub.Api.Models;
using MKasperczyk.GitHub.Api.Services;
using MKasperczyk.GitHub.Api.Services.Contract;
using MKasperczyk.GitHub.Api.Tests.Helpers;
using MKasperczyk.GitHub.Api.Tests.Mocks;
using MKasperczyk.GitHub.Api.Tests.Mocks.Models;
using Moq;
using System.Net.Http.Json;

namespace MKasperczyk.GitHub.Api.Tests
{
    public class GitHubServiceTests
    {
        [Fact]
        public async Task GetRepositoryInfosByOwnerAsyncShouldReturnCorrectValues()
        {
            // given
            var mockData = GitHubApiMock.GetRepositoryInfo();
            IGitHubService gitHubService = GetService(mockData);

            // when
            var result = await gitHubService.GetRepositoryInfosByOwnerAsync("owner");

            // then
            Assert.Equivalent(new List<RepositoryInfo>()
            {
                new RepositoryInfo()
                {
                    Id = 1,
                    Name = "TestRepo",
                    Size = 13,
                    ForksCount = 2,
                    StargazersCount = 8,
                    WatchersCount = 6
                },
                new RepositoryInfo()
                {
                    Id = 2,
                    Name = "OtherRepo",
                    Size = 17,
                    ForksCount = 4,
                    StargazersCount = 18,
                    WatchersCount = 1
                }
            }, result);
        }

        [Fact]
        public async Task GetRepositoryDetailInfosByOwnerAsyncShouldReturnCorrectValues()
        {
            // given
            var mockData = GitHubApiMock.GetRepositoryDetailInfo();
            IGitHubService gitHubService = GetService(mockData);

            // when
            var result = await gitHubService.GetRepositoryDetailInfosByOwnerAsync("owner");

            // then
            Assert.Equivalent(new List<RepositoryDetailInfo>()
            {
                new RepositoryDetailInfo()
                {
                    Id = 1,
                    Name = "TestRepo",
                    Size = 13,
                    ForksCount = 2,
                    StargazersCount = 8,
                    WatchersCount = 6,
                    GitUrl = "gitTestRepo_url",
                    HtmlUrl = "htmlTestRepo_url",
                    Owner = new RepositoryDetailOwnerInfo()
                    {
                        AvatarUrl = "awatarTestRepo_url"
                    },
                    CreatedAt = new DateTime(2021, 2, 15),
                    PushedAt = new DateTime(2021, 2, 22),
                    UpdatedAt = new DateTime(2021, 2, 23)
                },
                new RepositoryDetailInfo()
                {
                    Id = 2,
                    Name = "OtherRepo",
                    Size = 17,
                    ForksCount = 4,
                    StargazersCount = 18,
                    WatchersCount = 1,
                    GitUrl = "gitOtherRepo_url",
                    HtmlUrl = "htmlOtherRepo_url",
                    Owner = new RepositoryDetailOwnerInfo()
                    {
                        AvatarUrl = "awatarOtherRepo_url"
                    },
                    CreatedAt = new DateTime(2021, 3, 15),
                    PushedAt = new DateTime(2021, 3, 22),
                    UpdatedAt = new DateTime(2021, 3, 23)
                }
            }, result);
        }

        private IGitHubService GetService(IEnumerable<RepositoryInfoHttpRequest> repositoryInfo)
        {
            var mockHandler = HttpClientHelper.GetResults(repositoryInfo);

            var httpClient = new HttpClient(mockHandler.Object);
            httpClient.BaseAddress = new Uri("https://127.0.0.1");

            return new GitHubService(httpClient, new Mock<ILogger<GitHubService>>().Object);
        }

        private IGitHubService GetService(IEnumerable<RepositoryDetailInfoHttpRequest> repositoryDetailInfo)
        {
            var mockHandler = HttpClientHelper.GetResults(repositoryDetailInfo);

            var httpClient = new HttpClient(mockHandler.Object);
            httpClient.BaseAddress = new Uri("https://127.0.0.1");

            return new GitHubService(httpClient, new Mock<ILogger<GitHubService>>().Object);
        }
    }
}