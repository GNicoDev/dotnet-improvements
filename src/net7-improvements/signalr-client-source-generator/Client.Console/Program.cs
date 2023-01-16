// See https://aka.ms/new-console-template for more information

using System.Reflection.Metadata;
using Client.Console.Configurations;
using Client.Console.Extensions;
using Client.Console.HubClients;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Server.Contracts;
using Server.Contracts.ChatHub;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var serviceCollection = new ServiceCollection()
    .Configure<HubConfiguration>(configuration.GetSection(nameof(HubConfiguration)));

var serviceProvider = serviceCollection.BuildServiceProvider();
var hubOpt = serviceProvider.GetRequiredService<IOptionsMonitor<HubConfiguration>>();
if (hubOpt == null)
    throw new Exception($"Не указана конфигурация приложения {nameof(HubConfiguration)}");
var hubCfg = hubOpt.CurrentValue;
var cts = new CancellationTokenSource();
var ct = cts.Token;

if (string.IsNullOrWhiteSpace(hubCfg.CurrentUserName))
{
    Console.WriteLine("Напишите свое имя:");
    var userName = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(userName))
    {
        Console.WriteLine("Вы ввели пустое имя. Это недопустимо \n Введите еще раз:");
        userName = Console.ReadLine();
    }

    hubCfg.CurrentUserName = userName;
}
else
{
    Console.WriteLine($"Здравствуйте {hubCfg.CurrentUserName}");
}

var _hubConnection = new HubConnectionBuilder()
    .WithUrl(hubCfg.ChatHubAddress, opt =>
    {
        opt.Headers.Add(new (Constants.Headers.UserNameHeader, hubCfg.CurrentUserName));
    })
    .WithAutomaticReconnect()
    .Build();
var _serverConnection = _hubConnection
    .ServerProxy<IChatHubServer>();
var _clientConnection = _hubConnection
    .ClientRegistration<IChatHubClient>(new ChatHubHubClient(hubOpt));
            
await _hubConnection.StartAsync(ct);

while (!ct.IsCancellationRequested)
{
    var message = Console.ReadLine();
    if(!string.IsNullOrWhiteSpace(message))
        await _serverConnection.SendMessageAsync(hubCfg.CurrentUserName, message);
}
