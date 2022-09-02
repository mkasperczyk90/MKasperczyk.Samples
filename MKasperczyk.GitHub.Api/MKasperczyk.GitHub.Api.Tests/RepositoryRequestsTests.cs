using System.Net.Http.Json;

namespace MKasperczyk.GitHub.Api.Tests
{
    public class RepositoryRequestsTests
    {
        [Fact]
        public async Task Test1()
        {
            await using var application = new GitHubApplication();

            var client = application.CreateClient();
            var todos = await client.GetFromJsonAsync<object>("/todos");

            //Assert.Empty(todos);

        }
    }
}