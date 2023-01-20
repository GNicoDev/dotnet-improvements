namespace Client.Console.SignalR.Abstractions;

public interface ISignalRRegistrator
{
    bool DoRegistration();

    Task StartAsync(CancellationToken ct);

    Task SendMessageAsync(string user, string message);
}