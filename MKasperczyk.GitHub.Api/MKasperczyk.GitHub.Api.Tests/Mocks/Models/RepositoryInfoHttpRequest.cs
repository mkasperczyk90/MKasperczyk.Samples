using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MKasperczyk.GitHub.Api.Tests.Mocks.Models
{
    public class RepositoryInfoHttpRequest
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public long Stargazers_count { get; set; }

        public long Watchers_count { get; set; }

        public long Forks_count { get; set; }

        public long Size { get; set; }

        public DateTime Created_at { get; set; }

        public DateTime Pushed_at { get; set; }
    }
}
