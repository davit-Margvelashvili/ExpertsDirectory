using ExpertsDirectory.Models.Exceptions;
using ExpertsDirectory.Service.Abstractions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ExpertsDirectory.Service.Implementations
{
    internal sealed class WebSiteParser : IWebSiteParser
    {
        public async Task<List<string>> GetTagsAsync(string url, CancellationToken cancellationToken)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                throw new DomainException($"Url '{url}' is not well formed absolute url");

            var doc = await new HtmlWeb()
                .LoadFromWebAsync(url, cancellationToken)
                .ConfigureAwait(false);

            return doc is null
                ? new List<string>(0)
                : ParseNode("//h1").Concat(ParseNode("//h2")).Concat(ParseNode("//h3")).ToList();

            IEnumerable<string> ParseNode(string xpath) =>
                doc
                    .DocumentNode
                    .SelectNodes(xpath)
                    ?.Select(node => HttpUtility.HtmlDecode(node.InnerText))
                ?? Array.Empty<string>();
        }
    }
}