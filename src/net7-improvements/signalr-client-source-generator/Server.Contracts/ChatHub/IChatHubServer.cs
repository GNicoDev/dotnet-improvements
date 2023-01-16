namespace Server.Contracts.ChatHub;

public interface IChatHubServer
{
    Task SendMessageAsync(string user, string message);
}