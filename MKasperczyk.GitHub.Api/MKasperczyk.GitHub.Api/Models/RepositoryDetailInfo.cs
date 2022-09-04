using System.Text.Json.Serialization;

namespace MKasperczyk.GitHub.Api.Models
{
    public class RepositoryDetailInfo
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("git_url")]
        public string? GitUrl { get; set; }

        [JsonPropertyName("owner")]
        public RepositoryDetailOwnerInfo? Owner { get; set; }

        [JsonPropertyName("html_url")]
        public string? HtmlUrl { get; set; }

        [JsonPropertyName("stargazers_count")]
        public long StargazersCount { get; set; }

        [JsonPropertyName("watchers_count")]
        public long WatchersCount { get; set; }

        [JsonPropertyName("forks_count")]
        public long ForksCount { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("pushed_at")]
        public DateTime PushedAt { get; set; }
    }

    public class RepositoryDetailOwnerInfo 
    {
        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
    }
}
