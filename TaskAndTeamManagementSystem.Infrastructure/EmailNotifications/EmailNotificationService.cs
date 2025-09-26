using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;

namespace TaskAndTeamManagementSystem.Infrastructure.EmailNotifications;

public class EmailNotificationService : IEmailNotificationService
{
    public Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailAsync(string to, string subject, string body)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string attachmentName)
    {
        throw new NotImplementedException();
    }
}
