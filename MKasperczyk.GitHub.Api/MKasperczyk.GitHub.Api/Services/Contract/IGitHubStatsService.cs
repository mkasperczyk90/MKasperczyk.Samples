using MKasperczyk.GitHub.Api.Models;

namespace MKasperczyk.GitHub.Api.Services.Contract;

public interface IGitHubStatsService
{
    Task<RepositoryStatsInfo?> GetRepositoryStatsByOwnerAsync(string owner);
}
