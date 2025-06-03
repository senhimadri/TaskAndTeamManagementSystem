namespace TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;

public interface IRealTimeNotificationService
{
    Task SendChatMessageAsync(Guid toUserId, string message);
    Task SendNotificationAsync(Guid toUserId, string notification);
}
