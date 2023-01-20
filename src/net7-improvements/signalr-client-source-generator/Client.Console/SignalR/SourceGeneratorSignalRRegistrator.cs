using Client.Console.Configurations;
using Client.Console.Extensions;
using Client.Console.HubClients;
using Client.Console.SignalR.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Server.Contracts.ChatHub;
using Server.Contracts;

namespace Client.Console.SignalR;

public class SourceGeneratorSignalRRegistrator : ISignalRRegistrator
{
    private readonly IOptionsMonitor<HubConfiguration> _hubOptions;
    private HubConnection _hubConnection;
    private IChatHubServer _serverConnection;
    
    public SourceGeneratorSignalRRegistrator(IOptionsMonitor<HubConfiguration> hubOptions)
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
        _serverConnection = _hubConnection
            .ServerProxy<IChatHubServer>();
        _hubConnection.ClientRegistration<IChatHubClient>(new ChatHubHubClient(_hubOptions));
        
        return true;
    }

    public Task StartAsync(CancellationToken ct) =>
        _hubConnection.StartAsync(ct);

    public Task SendMessageAsync(string user, string message) =>
        _serverConnection.SendMessageAsync(user, message);
}