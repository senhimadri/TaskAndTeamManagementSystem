using Microsoft.AspNetCore.SignalR;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Infrastructure.PushNotifications;

namespace TaskAndTeamManagementSystem.Infrastructure;

public class SignalRNotifier(IHubContext<NotificationHub> _hubContext) : IRealTimeNotificationService
{
    public async Task SendChatMessageAsync(Guid toUserId, string message) 
        => await _hubContext.Clients.User(toUserId.ToString()).SendAsync("ReceiveChatMessage", message);


    public async Task SendNotificationAsync(Guid toUserId, string notification)
        => await _hubContext.Clients.User(toUserId.ToString()).SendAsync("ReceiveNotification", notification);

}
