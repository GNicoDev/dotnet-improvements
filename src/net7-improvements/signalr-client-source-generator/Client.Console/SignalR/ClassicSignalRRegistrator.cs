using Client.Console.Configurations;
using Client.Console.SignalR.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Server.Contracts;

namespace Client.Console.SignalR;

public class ClassicSignalRRegistrator : ISignalRRegistrator 
{
    private readonly IOptionsMonitor<HubConfiguration> _hubOptions;
    private HubConnection _hubConnection;
    
    public ClassicSignalRRegistrator(IOptionsMonitor<HubConfiguration> hubOptions)
    {
        _hubOptions = hubOptions;
    }
    
    public bool DoRegistration()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubOptions.CurrentValue.ChatHubAddress, opt =>
            {
                opt.Headers.Add(new (Constants.Headers.UserNameHeader, 
                    _hubOptions.CurrentValue.CurrentUserName));
            })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string, string>("ReceiveMessageAsync", (user, message) =>
        {
            if (_hubOptions.CurrentValue.CurrentUserName?.Equals(user) ?? false)
            {
                System.Console.WriteLine(message);
                return Task.CompletedTask;
            }
            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine($"{user} =>\n {message}");
            System.Console.ResetColor();
            return Task.CompletedTask;
        });
        
        _hubConnection.On<string>("UserAddedToChat", (user) =>
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"---------------------------------");
            System.Console.WriteLine($"Пользователь {user} добавлен в чат");
            System.Console.WriteLine($"---------------------------------");
            System.Console.ResetColor();
            return Task.CompletedTask;
        });
        
        _hubConnection.On<string>("UserRemovedFromChat", (user) =>
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"---------------------------------");
            System.Console.WriteLine($"Пользователь {user} убыл из чата");
            System.Console.WriteLine($"---------------------------------");
            System.Console.ResetColor();
            return Task.CompletedTask;
        });
        
            
        return true;
    }

    public Task StartAsync(CancellationToken ct) =>
        _hubConnection.StartAsync(ct);

    public Task SendMessageAsync(string user, string message) =>
        _hubConnection.InvokeAsync("SendMessageAsync", user, message);

}