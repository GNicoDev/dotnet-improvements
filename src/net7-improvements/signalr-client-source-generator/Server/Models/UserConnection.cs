namespace Server.Models;

/// <summary>
/// Описание соединения пользователя в чате
/// </summary>
public class UserConnection
{
    public UserConnection(string connectionId)
    {
        ConnectionId = connectionId;
        JoinedAt = DateTime.UtcNow;
    }
    
    public UserConnection(string connectionId, DateTime joinedAt)
    {
        ConnectionId = connectionId;
        JoinedAt = joinedAt;
    }

    /// <summary>
    /// Идентификатор соединения
    /// </summary>
    public string ConnectionId { get; }

    /// <summary>
    /// Время присоединения
    /// </summary>
    public DateTime JoinedAt { get; }
}