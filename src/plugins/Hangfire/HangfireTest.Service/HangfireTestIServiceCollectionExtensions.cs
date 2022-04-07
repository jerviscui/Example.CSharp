using Microsoft.Extensions.DependencyInjection;

namespace HangfireTest.Service;

public static class HangfireTestIServiceCollectionExtensions
{
    public static IServiceCollection AddTestJobs(this IServiceCollection services)
    {
        services.AddTransient<DefaultJobs>();

        return services;
    }
}
