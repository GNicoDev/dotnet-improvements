using Server.Hubs;

namespace Server.Extensions;

/// <summary>
/// Класс с расширениями для типа <see cref="WebApplication"/>
/// </summary>
public static class WebApplicationExtensions
{
    public static WebApplication MapSignalRHubs(this WebApplication application)
    {
        application.MapHub<ChatHub>("/chat-hub");
        
        return application;
    }
}