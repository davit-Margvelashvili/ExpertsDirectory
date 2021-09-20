using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpertsDirectory.Service.Abstractions
{
    public interface IWebSiteParser
    {
        Task<List<string>> GetTagsAsync(string url, CancellationToken cancellationToken);
    }
}