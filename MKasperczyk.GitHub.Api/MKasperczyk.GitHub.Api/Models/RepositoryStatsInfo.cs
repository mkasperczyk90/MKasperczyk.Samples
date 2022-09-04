namespace MKasperczyk.GitHub.Api.Models
{
    public record RepositoryStatsInfo(string Owner, IDictionary<char, int> Letters, double AvgStargazers, double AvgWatchers, double AvgForks, double AvgSize, int repositoryCount);
}
