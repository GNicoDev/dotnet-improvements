using Microsoft.AspNetCore.SignalR.Client;
using Server.Contracts.Attributes;

namespace Client.Console.Extensions;

public static partial class HubRegistrationExtensions
{
    [HubClientProxy]
    public static partial IDisposable ClientRegistration<T>(this HubConnection connection, T provider);

    [HubServerProxy]
    public static partial T ServerProxy<T>(this HubConnection connection);
}