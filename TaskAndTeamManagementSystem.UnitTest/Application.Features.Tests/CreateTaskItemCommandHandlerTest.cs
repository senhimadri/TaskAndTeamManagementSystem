using Moq;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;

namespace TaskAndTeamManagementSystem.UnitTest.Application.Features.Tests;

public class CreateTaskItemCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IRealTimeNotificationService> _mockRealTimeNotificationService = new();
    private readonly Mock<ICurrentUserService> _mockCurrentUser = new();
    private readonly Mock<IEventPublisher> _mockEventPublisher = new();
    private readonly CreateTaskItemCommandHandler _handler;

    public CreateTaskItemCommandHandlerTest()
    {
        _mockCurrentUser.Setup(x => x.UserId).Returns(Guid.NewGuid());

        _handler = new CreateTaskItemCommandHandler(
            _mockUnitOfWork.Object,
            _mockRealTimeNotificationService.Object,
            _mockCurrentUser.Object,
            _mockEventPublisher.Object
        );
    }
}