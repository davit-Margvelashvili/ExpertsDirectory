using ExpertsDirectory.Database;
using ExpertsDirectory.Service.Abstractions;
using ExpertsDirectory.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace ExpertsDirectory.Service
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddExpertsDirectoryServices(this IServiceCollection self)
        {
            return self
                .AddScoped<IMemberService, MemberService>()
                .AddScoped<IWebSiteParser, WebSiteParser>()
                .AddDbContext<ExpertsDirectoryContext>();
        }
    }
}