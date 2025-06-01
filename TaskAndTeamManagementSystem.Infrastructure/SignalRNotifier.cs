using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Infrastructure.PushNotifications;

namespace TaskAndTeamManagementSystem.Infrastructure;

internal class SignalRNotifier(IHubContext<NotificationHub> hubContext) : IRealTimeNotificationService
{

    public async Task SendChatMessageAsync(Guid toUserId, string message)
    {
        await hubContext.Clients.User(toUserId.ToString()).SendAsync("ReceiveNotification", message);

    }

    public async Task SendNotificationAsync(Guid toUserId, string notification)
    {
        await hubContext.Clients.User(toUserId.ToString()).SendAsync("ReceiveNotification", notification);
    }
}
