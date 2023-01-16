namespace Server.Models;

public class ChatUser
{
    private List<UserConnection> _connections;

    public ChatUser(string userName)
    {
        _connections = new List<UserConnection>();
        UserName = userName;
    }
    
    public ChatUser(string userName, string connectionId)
    {
        _connections = new (){new UserConnection(connectionId)};
        UserName = userName;
    }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; }
    
    /// <summary>
    /// Идентификаторы соединений пользователя с разных устройств
    /// </summary>
    public IReadOnlyCollection<UserConnection> Connections => _connections;

    /// <summary>
    /// Добавление нового соединения для пользователя
    /// </summary>
    /// <param name="connectionId">Идентификато соединения</param>
    public void AddConnection(string connectionId)
    {
        if(_connections.Any(it => it.ConnectionId == connectionId))
            return;
        _connections.Add(new UserConnection(connectionId, DateTime.UtcNow));
    }

    /// <summary>
    /// Удаление соединения из списка
    /// </summary>
    /// <param name="connectionId">Идентификато соединения</param>
    public void RemoveConnection(string connectionId)
    {
        var connection = _connections.FirstOrDefault(it => it.ConnectionId == connectionId); 
        if(connection == null)
            return;
        _connections.Remove(connection);
    }
}