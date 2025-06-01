using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskAndTeamManagementSystem.Infrastructure.PushNotifications;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User connected: {userId} ({Context.ConnectionId})");
        await base.OnConnectedAsync();
    }
}
