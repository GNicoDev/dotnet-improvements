using Client.Console.Configurations;
using Microsoft.Extensions.Options;
using Server.Contracts.ChatHub;

namespace Client.Console.HubClients;

/// <inheritdoc cref="IChatHubClient"/>
public class ChatHubHubClient : IChatHubClient
{
    private HubConfiguration _hubConfiguration;
    
    public ChatHubHubClient(IOptionsMonitor<HubConfiguration> hubConfiguration)
    {
        _hubConfiguration = hubConfiguration.CurrentValue;
        hubConfiguration.OnChange(configuration => _hubConfiguration = configuration);
    }
    
    /// <inheritdoc cref="IChatHubClient.ReceiveMessageAsync"/>
    public Task ReceiveMessageAsync(string user, string message)
    {
        if (_hubConfiguration.CurrentUserName?.Equals(user) ?? false)
        {
            System.Console.WriteLine(message);
            return Task.CompletedTask;
        }
        System.Console.ForegroundColor = ConsoleColor.Blue;
        System.Console.WriteLine($"{user} =>\n {message}");
        System.Console.ResetColor();
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IChatHubClient.UserAddedToChat"/>
    public Task UserAddedToChat(string user)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"---------------------------------");
        System.Console.WriteLine($"Пользователь {user} добавлен в чат");
        System.Console.WriteLine($"---------------------------------");
        System.Console.ResetColor();
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IChatHubClient.UserRemovedFromChat"/>
    public Task UserRemovedFromChat(string user)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"---------------------------------");
        System.Console.WriteLine($"Пользователь {user} убыл из чата");
        System.Console.WriteLine($"---------------------------------");
        System.Console.ResetColor();
        return Task.CompletedTask;
    }
}