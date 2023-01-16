using Server.Contracts.ChatHub;

namespace Server.Hubs.Abstractions;

/// <summary>
/// Объединяющий интерфейс разделения потоков к серверу и от сервера
/// </summary>
public interface IChatHub : IChatHubServer, IChatHubClient
{ }