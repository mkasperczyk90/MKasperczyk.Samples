using MKasperczyk.GitHub.Api.Models;
using MKasperczyk.GitHub.Api.Services.Contract;
using System.Net.Http;

namespace MKasperczyk.GitHub.Api.Services
{
    public class GitHubStatsService : IGitHubStatsService
    {
        private readonly IGitHubService _gitHubService;
        private readonly ILogger<GitHubStatsService> _logger;

        public GitHubStatsService(IGitHubService gitHubService, ILogger<GitHubStatsService> logger)
        {
            _gitHubService = gitHubService;
            _logger = logger;
        }

        public async Task<RepositoryStatsInfo?> GetRepositoryStatsByOwnerAsync(string owner)
        {
            if (string.IsNullOrEmpty(owner)) return null;

            var repositories = await _gitHubService.GetRepositoryInfosByOwnerAsync(owner);

            var avgForks = repositories.Average(repo => repo.ForksCount);
            var avgSize = repositories.Average(repo => repo.Size);
            var avgStargazers = repositories.Average(repo => repo.StargazersCount);
            var avgWatchers = repositories.Average(repo => repo.WatchersCount);
            var letters = GetLettersCount(repositories);

            return new RepositoryStatsInfo(owner, letters, avgStargazers, avgWatchers, avgForks, avgSize, repositories.Count());
        }

        private Dictionary<char, int> GetLettersCount(IEnumerable<RepositoryInfo> repositories)
        {
            var lettersCount = new Dictionary<char, int>();

            foreach (var repository in repositories)
            {
                if (string.IsNullOrEmpty(repository.Name)) break;

                var lowerCaseName = repository.Name.ToLower();
                foreach (char letter in lowerCaseName)
                {
                    if (lettersCount.ContainsKey(letter))
                    {
                        lettersCount[letter]++;
                    } else
                    {
                        lettersCount.Add(letter, 1);
                    }
                }
            }
            return lettersCount;
        }
    }
}
