using MassTransit;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;

namespace TaskAndTeamManagementSystem.Infrastructure.MessageBrokers.Publishers;

internal class MassTransitEventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public Task PublishAsync<T>(T message) where T : class => publishEndpoint.Publish(message);
}
