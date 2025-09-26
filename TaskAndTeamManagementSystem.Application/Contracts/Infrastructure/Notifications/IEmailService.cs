namespace TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;

public interface IEmailNotificationService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string attachmentName);
    Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body);

}
