namespace Server.Contracts.ChatHub;

/// <summary>
/// Интерфейс для клинтского подключения к хабу
/// </summary>
public interface IChatHubClient
{
    /// <summary>
    /// Получение нового сообщения
    /// </summary>
    /// <param name="user">От кого</param>
    /// <param name="message">Сообщение</param>
    Task ReceiveMessageAsync(string user, string message);

    /// <summary>
    /// Пользователь добавлен в чат
    /// </summary>
    /// <param name="user">Имя пользователя</param>
    Task UserAddedToChat(string user);

    /// <summary>
    /// Пользователь удален из чата
    /// </summary>
    /// <param name="user">Имя пользователя</param>
    Task UserRemovedFromChat(string user);
}