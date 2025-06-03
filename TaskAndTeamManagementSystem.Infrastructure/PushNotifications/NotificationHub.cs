using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TaskAndTeamManagementSystem.Infrastructure.PushNotifications;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {

        Console.WriteLine($"✅ SignalR Connected: {Context.UserIdentifier} - {Context.ConnectionId}");

        await base.OnConnectedAsync();
    }
}
