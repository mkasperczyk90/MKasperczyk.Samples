using MKasperczyk.GitHub.Api.Services.Contract;
using MKasperczyk.GitHub.Api.Services;
using Microsoft.Extensions.Options;
using MKasperczyk.GitHub.Api.Options;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;
using System.Net.Http;

namespace MKasperczyk.GitHub.Api.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddGitHubServices(
            this IServiceCollection services,
            ConfigurationManager configuration
            )
        {
            GitHubOptions options = RegisterAndGetOptions(services, configuration);
            Uri baseGitHubUrl = new Uri(options.ApiUrl);

            services.AddSingleton<IGitHubStatsService, GitHubStatsService>();

            services.AddSingleton<IGitHubService, GitHubService>();
            services.AddHttpClient<IGitHubService, GitHubService>(client =>
            {
                client.BaseAddress = baseGitHubUrl;
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
                client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Other");
            });

            return services;
        }

        private static GitHubOptions RegisterAndGetOptions(IServiceCollection services, ConfigurationManager configuration)
        {
            var gitHubSettingsSection = configuration.GetSection(GitHubOptions.GitHubOptionName);
            var options = gitHubSettingsSection.Get<GitHubOptions>();
            services.Configure<GitHubOptions>(gitHubSettingsSection);
            return options;
        }
    }
}
