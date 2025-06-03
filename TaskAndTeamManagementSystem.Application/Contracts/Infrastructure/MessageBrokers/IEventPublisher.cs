namespace TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;

public interface IEventPublisher
{
    Task PublishAsync<T>(T message) where T : class;
}
