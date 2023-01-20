using Client.Console.Configurations;
using Client.Console.SignalR;
using Client.Console.SignalR.Abstractions;
using Microsoft.Extensions.Options;

namespace Client.Console.Factories;

internal class SignalRRegistrationFactory
{
    private readonly IOptionsMonitor<HubConfiguration> _hubOptions;

    public SignalRRegistrationFactory(IOptionsMonitor<HubConfiguration> hubOptions)
    {
        _hubOptions = hubOptions;
    }
    
    public ISignalRRegistrator RegisterSignalRConnection(SignalRRegistrationType registrationType) =>
        registrationType switch
        {
            SignalRRegistrationType.Classic => new ClassicSignalRRegistrator(_hubOptions),
            SignalRRegistrationType.SourceGenerator => new SourceGeneratorSignalRRegistrator(_hubOptions),
            _ => throw new Exception("Нет других способов регистрации")
        };
}