using Server.Models;

namespace Server.Services.Abstractions;

/// <summary>
/// Сервис управляющий чатом и его пользователями
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Получить пользователя по идентификатору соединения
    /// </summary>
    /// <param name="connectionId">Идентификатор соединения</param>
    /// <returns>Пользователь или Null если не найден</returns>
    Task<ChatUser?> GetUserByConnectionIdAsync(string connectionId);

    /// <summary>
    /// Получение пользователя чата по нику
    /// </summary>
    /// <param name="userName">Ник пользователя</param>
    /// <returns>Пользователь или Null</returns>
    Task<ChatUser?> GetUserAsync(string userName);

    /// <summary>
    /// Добавление нового пользователя и соединения
    /// </summary>
    /// <param name="userName">Имя пользователя</param>
    /// <param name="connectionId">Номер соединения</param>
    Task AddUserToChatAsync(string userName, string connectionId);

    /// <summary>
    /// Добавление нового соединения пользователю
    /// </summary>
    /// <param name="user">Пользователь которому добавляется соединение</param>
    /// <param name="connectionId">Идентификатор соединения</param>
    Task AddConnectionAsync(ChatUser user, string connectionId);

    /// <summary>
    /// Удаление соединения у пользователю
    /// </summary>
    /// <param name="user">Пользователь у которого удаляется соединение</param>
    /// <param name="connectionId">Идентификатор соединения</param>
    Task RemoveConnectionAsync(ChatUser user, string connectionId);
}