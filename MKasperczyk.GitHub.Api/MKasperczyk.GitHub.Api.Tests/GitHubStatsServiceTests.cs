using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using MKasperczyk.GitHub.Api.Models;
using MKasperczyk.GitHub.Api.Services;
using MKasperczyk.GitHub.Api.Services.Contract;
using Moq;
using System.Net.Http.Json;

namespace MKasperczyk.GitHub.Api.Tests
{
    public class GitHubStatsServiceTests
    {
        private readonly Mock<IGitHubService> _gitHubService;
        private readonly IGitHubStatsService _gitHubStatsService;

        public GitHubStatsServiceTests()
        {
            var logger = new Mock<ILogger<GitHubStatsService>>();
            _gitHubService = new Mock<IGitHubService>();
            _gitHubStatsService = new GitHubStatsService(_gitHubService.Object, logger.Object);
        }


        [Fact]
        public async Task GetRepositoryStatsByOwnerAsyncShouldReturnOwner()
        {
            // given
            _gitHubService
                .Setup(s => s.GetRepositoryInfosByOwnerAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    new List<RepositoryInfo>
                    {
                        new RepositoryInfo { Id = 1, Name = "name" },
                        new RepositoryInfo { Id = 2, Name = "name" }
                    });

            // when
            var result = await _gitHubStatsService.GetRepositoryStatsByOwnerAsync("owner");

            // then
            Assert.Same(result?.Owner, "owner");
        }

        [Fact]
        public async Task GetRepositoryStatsByOwnerAsyncShouldCountsAlphabetAsLowercaseLetterCorrectly()
        {
            // given
            _gitHubService
                .Setup(s => s.GetRepositoryInfosByOwnerAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    new List<RepositoryInfo>
                    {
                        new RepositoryInfo { Id = 1, Name = "AlPhaBet" },
                        new RepositoryInfo { Id = 2, Name = "NamE" },
                        new RepositoryInfo { Id = 3, Name = "TeStY" }
                    });

            // when
            var result = await _gitHubStatsService.GetRepositoryStatsByOwnerAsync("owner");

            // then
            Assert.NotNull(result);
            Assert.Equivalent(new Dictionary<char, int>()
            {
                { 'a', 3 },
                { 'l', 1 },
                { 'p', 1 },
                { 'h', 1 },
                { 'b', 1 },
                { 'e', 3 },
                { 't', 3 },
                { 'n', 1 },
                { 'm', 1 },
                { 's', 1 },
                { 'y', 1 },
            }, result?.Letters);
        }
        [Fact]
        public async Task GetRepositoryStatsByOwnerAsyncShouldCorrectlyCalculateAverages()
        {
            // given
            _gitHubService
                .Setup(s => s.GetRepositoryInfosByOwnerAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    new List<RepositoryInfo>
                    {
                        new RepositoryInfo { Id = 1, WatchersCount = 8, StargazersCount = 17, ForksCount = 54, Size = 32 },
                        new RepositoryInfo { Id = 2, WatchersCount = 6, StargazersCount = 7, ForksCount = 11, Size = 40 },
                        new RepositoryInfo { Id = 3, WatchersCount = 17, StargazersCount = 19, ForksCount = 17, Size = 64 }
                    });

            // when
            var result = await _gitHubStatsService.GetRepositoryStatsByOwnerAsync("owner");

            // then
            Assert.NotNull(result);
            Assert.Equal(14.333333333333334, result.AvgStargazers, precision: 15);
            Assert.Equal(10.333333333333334, result.AvgWatchers, precision: 15);
            Assert.Equal(45.333333333333336, result.AvgSize, precision: 15);
            Assert.Equal(27.333333333333332, result.AvgForks, precision: 15);
        }
    }
}