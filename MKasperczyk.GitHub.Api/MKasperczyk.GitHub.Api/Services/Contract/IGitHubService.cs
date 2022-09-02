using MKasperczyk.GitHub.Api.Models;

namespace MKasperczyk.GitHub.Api.Services.Contract;

public interface IGitHubService
{
    Task<IEnumerable<RepositoryInfo>> GetRepositoryInfosByOwnerAsync(string owner);
    Task<IEnumerable<RepositoryDetailInfo>> GetRepositoryDetailInfosByOwnerAsync(string owner);
}

