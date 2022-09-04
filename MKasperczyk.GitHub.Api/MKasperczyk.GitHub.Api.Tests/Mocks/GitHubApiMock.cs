using MKasperczyk.GitHub.Api.Models;
using MKasperczyk.GitHub.Api.Tests.Mocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MKasperczyk.GitHub.Api.Tests.Mocks
{
    public class GitHubApiMock
    {
        public static IEnumerable<RepositoryInfoHttpRequest> GetRepositoryInfo()
        {
            return new List<RepositoryInfoHttpRequest>
            {
                new RepositoryInfoHttpRequest()
                {
                    Id = 1,
                    Name = "TestRepo",
                    Size = 13,
                    Forks_count = 2,
                    Stargazers_count = 8,
                    Watchers_count = 6
                },
                new RepositoryInfoHttpRequest()
                {
                    Id = 2,
                    Name = "OtherRepo",
                    Size = 17,
                    Forks_count = 4,
                    Stargazers_count = 18,
                    Watchers_count = 1
                }
            };
        }
        public static IEnumerable<RepositoryDetailInfoHttpRequest> GetRepositoryDetailInfo()
        {
            return new List<RepositoryDetailInfoHttpRequest>
            {
                new RepositoryDetailInfoHttpRequest()
                {
                    Id = 1,
                    Name = "TestRepo",
                    Size = 13,
                    Forks_count = 2,
                    Stargazers_count = 8,
                    Watchers_count = 6,
                    Git_url = "gitTestRepo_url",
                    Html_url = "htmlTestRepo_url",
                    Owner = new RepositoryDetailOwnerInfoHttpRequest()
                    {
                        Avatar_url = "awatarTestRepo_url"
                    },
                    Created_at = new DateTime(2021, 2, 15),
                    Pushed_at = new DateTime(2021, 2, 22),
                    Updated_at = new DateTime(2021, 2, 23)
                },
                new RepositoryDetailInfoHttpRequest()
                {
                    Id = 2,
                    Name = "OtherRepo",
                    Size = 17,
                    Forks_count = 4,
                    Stargazers_count = 18,
                    Watchers_count = 1,
                    Git_url = "gitOtherRepo_url",
                    Html_url = "htmlOtherRepo_url",
                    Owner = new RepositoryDetailOwnerInfoHttpRequest()
                    {
                        Avatar_url = "awatarOtherRepo_url"
                    },
                    Created_at = new DateTime(2021, 3, 15),
                    Pushed_at = new DateTime(2021, 3, 22),
                    Updated_at = new DateTime(2021, 3, 23)
                }
            };
        }

        
    }
}