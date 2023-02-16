#region common registration

using Client.Console.Configurations;
using Client.Console.Extensions;
using Client.Console.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var serviceCollection = new ServiceCollection()
    .AddCustomOptions(configuration)
    .AddSingleton<SignalRRegistrationFactory>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var hubOpt = serviceProvider.GetRequiredService<IOptionsMonitor<HubConfiguration>>();
if (hubOpt == null)
    throw new Exception($"Не указана конфигурация приложения {nameof(HubConfiguration)}");
var hubCfg = hubOpt.CurrentValue;

var appOpt = serviceProvider.GetRequiredService<IOptionsMonitor<AppConfiguration>>();
if (appOpt == null)
    throw new Exception($"Не указана конфигурация приложения {nameof(AppConfiguration)}");
var appCfg = appOpt.CurrentValue;

var cts = new CancellationTokenSource();
var ct = cts.Token;
#endregion

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

var factory = serviceProvider.GetRequiredService<SignalRRegistrationFactory>();
var connection = factory.RegisterSignalRConnection(appCfg.SignalRRegistrationType);
connection.DoRegistration();

await connection.StartAsync(ct);

while (!ct.IsCancellationRequested)
{
    var message = Console.ReadLine();
    if(!string.IsNullOrWhiteSpace(message))
        await connection.SendMessageAsync(hubCfg.CurrentUserName, message);
}
