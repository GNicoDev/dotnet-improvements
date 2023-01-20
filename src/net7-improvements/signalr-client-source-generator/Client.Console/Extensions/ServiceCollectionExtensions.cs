using Client.Console.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Console.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HubConfiguration>(configuration.GetSection(nameof(HubConfiguration)))
            .Configure<AppConfiguration>(configuration.GetSection(nameof(AppConfiguration)));

        return services;
    }
}