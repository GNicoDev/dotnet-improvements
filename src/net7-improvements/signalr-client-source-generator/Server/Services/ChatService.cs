using Server.Models;
using Server.Services.Abstractions;

namespace Server.Services;

/// <inheritdoc cref="IChatService"/>
public class ChatService : IChatService
{
    private readonly List<ChatUser> _chatUsers;

    public ChatService()
    {
        _chatUsers = new List<ChatUser>();
    }

    /// <inheritdoc cref="IChatService.GetUserByConnectionIdAsync"/>
    public Task<ChatUser?> GetUserByConnectionIdAsync(string connectionId) =>
        Task.FromResult(_chatUsers
            .FirstOrDefault(it => 
                it.Connections
                    .Any(c => c.ConnectionId == connectionId)));

    /// <inheritdoc cref="IChatService.GetUserAsync"/>
    public Task<ChatUser?> GetUserAsync(string userName) =>
        Task.FromResult(_chatUsers.FirstOrDefault(it => it.UserName.Equals(userName)));

    /// <inheritdoc cref="IChatService.AddUserToChatAsync"/>
    public Task AddUserToChatAsync(string userName, string connectionId)
    {
        _chatUsers.Add(new ChatUser(userName, connectionId));
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IChatService.AddConnectionAsync"/>
    public Task AddConnectionAsync(ChatUser user, string connectionId)
    {
        user.AddConnection(connectionId);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IChatService.RemoveConnectionAsync"/>
    public Task RemoveConnectionAsync(ChatUser user, string connectionId)
    {
        if (user.Connections.Count == 1)
        {
            _chatUsers.Remove(user);
            return Task.CompletedTask;
        }
        user.RemoveConnection(connectionId);
        return Task.CompletedTask;
    }
}