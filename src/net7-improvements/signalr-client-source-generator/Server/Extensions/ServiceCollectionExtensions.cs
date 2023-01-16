using Server.Services;
using Server.Services.Abstractions;

namespace Server.Extensions;

/// <summary>
/// Класс с расширениями для типа <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSignalRHubServices(this IServiceCollection services)
    {
        services.AddCors(setup =>
        {
            setup.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowed(_ => true)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });
        services.AddSignalR();
        
        return services;
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddSingleton<IChatService, ChatService>();
        
        return services;
    }
}