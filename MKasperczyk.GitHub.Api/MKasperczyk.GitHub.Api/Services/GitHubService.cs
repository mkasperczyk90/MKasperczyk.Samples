using MKasperczyk.GitHub.Api.Models;
using MKasperczyk.GitHub.Api.Services.Contract;
using System.Net.Http.Json;

namespace MKasperczyk.GitHub.Api.Services
{
    public class GitHubService : IGitHubService
    {
        private const int ResultsPerPage = 100;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GitHubService> _logger;

        public GitHubService(HttpClient httpClient, ILogger<GitHubService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner)
        {
            List<RepositoryInfo> repositories = new List<RepositoryInfo>();

            int pageNumber = 1;

            while (true)
            {
                var requestResult = await _httpClient.GetFromJsonAsync<IEnumerable<RepositoryInfo>>($"users/{owner}/repos?per_page={ResultsPerPage}&page={pageNumber++}");

                if (requestResult == null) break;
                repositories.AddRange(requestResult);

                // if there are less then max results, it is all data.
                if (requestResult.Count() != ResultsPerPage) break;
            }
            return repositories;
        }
        public async Task<IEnumerable<RepositoryDetailInfo>> GetRepositoryDetailInfosByOwnerAsync(string owner)
        {
            List<RepositoryDetailInfo> repositories = new List<RepositoryDetailInfo>();

            int pageNumber = 1;

            while (true)
            {
                var requestResult = await _httpClient.GetFromJsonAsync<IEnumerable<RepositoryDetailInfo>>($"users/{owner}/repos?per_page={ResultsPerPage}&page={pageNumber++}");

                if (requestResult == null) break;
                repositories.AddRange(requestResult);

                // if there are less then max results, it is all data.
                if (requestResult.Count() != ResultsPerPage) break;
            }
            return repositories;
        }
    }
}
