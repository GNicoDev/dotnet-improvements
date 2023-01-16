using Microsoft.AspNetCore.SignalR;
using Server.Contracts;
using Server.Hubs.Abstractions;
using Server.Services.Abstractions;

namespace Server.Hubs;

public class ChatHub : Hub<IChatHub>
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext is null)
            throw new Exception($"Не удалось получить объект типа {nameof(HttpContext)}");
        if (!httpContext.Request.Headers.TryGetValue(Constants.Headers.UserNameHeader, out var userName))
            throw new Exception("Не передано имя пользователя");
        if (string.IsNullOrWhiteSpace(userName))
            throw new Exception("Не передано имя пользователя");
        await UserAddedToChat(userName!, Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await UserRemovedFromChat(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public Task SendMessageAsync(string user, string message) =>
        this.Clients.Others.ReceiveMessageAsync(user, message);
    
    public async Task UserAddedToChat(string user, string connectionId)
    {
        var userInStore = await _chatService.GetUserAsync(user);
        await Groups.AddToGroupAsync(connectionId, user);
        if (userInStore is null)
        {
            await _chatService.AddUserToChatAsync(user, connectionId);
            await Clients.Others.UserAddedToChat(user);
        }
        else
            await _chatService.AddConnectionAsync(userInStore, connectionId);
    }

    public async Task UserRemovedFromChat(string connectionId)
    {
        var user = await _chatService.GetUserByConnectionIdAsync(connectionId);
        if (user is null)
            return;
        await _chatService.RemoveConnectionAsync(user, connectionId);
        await Groups.RemoveFromGroupAsync(connectionId, user.UserName);
        await Clients.Others.UserRemovedFromChat(user.UserName);
    }
}