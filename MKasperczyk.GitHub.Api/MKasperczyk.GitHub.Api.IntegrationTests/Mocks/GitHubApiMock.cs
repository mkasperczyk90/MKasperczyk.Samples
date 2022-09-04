using MKasperczyk.GitHub.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKasperczyk.GitHub.Api.IntegrationTests.Mocks
{
    public class GitHubApiMock
    {
        public static IEnumerable<RepositoryInfo> Get()
        {
            return new List<RepositoryInfo>
            {
                new RepositoryInfo()
                {
                    Id = 1,
                    Name = "TestRepo",
                    Size = 13
                }
            };
        }
    }
}
