using Moq;
using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;
using TaskAndTeamManagementSystem.Contracts;
using TaskAndTeamManagementSystem.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskAndTeamManagementSystem.UnitTest.Application.Features.Tests;

public class CreateTaskItemCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<ICurrentUserService> _mockCurrentUserService = new();
    private readonly Mock<IRealTimeNotificationService> _mockNotifier = new();
    private readonly Mock<IEventPublisher> _mockEventPublisher = new();

    private readonly CreateTaskItemCommandHandler _handler;

    public CreateTaskItemCommandHandlerTest()
    {
        _mockCurrentUserService.Setup(x => x.UserId).Returns(Guid.NewGuid());

        _handler = new CreateTaskItemCommandHandler(
            _mockUnitOfWork.Object,
            _mockNotifier.Object,
            _mockCurrentUserService.Object,
            _mockEventPublisher.Object
        );
    }

    private static CreateTaskItemPayload GetValidPayload() => new CreateTaskItemPayload
    (
        Title: "Test Task",
        Description: "This is a test task",
        Status: (int)ActivityStatus.ToDo,
        DueDate: DateTimeOffset.UtcNow.AddDays(7),
        AssignedUserId: Guid.NewGuid(),
        TeamId: 1 // Provide a valid TeamId value
    );

  

    [Fact]
    public async Task Handle_ValidPayload_ShouldReturnSuccess()
    {
        var payload = GetValidPayload();
        var command = new CreateTaskItemCommand { Payload = payload };

        _mockUnitOfWork.Setup(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                                        .ReturnsAsync((Expression<Func<Team, bool>> filter) =>
                                        {
                                            var compiled = filter.Compile();
                                            var team1 = new Team { Id = 1 };
                                            return compiled(team1);
                                        });

        _mockEventPublisher.Setup(x => x.PublishAsync(It.IsAny<CreateTaskItemEvent>())).Returns(Task.CompletedTask);

        _mockNotifier.Setup(x => x.SendNotificationAsync(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        _mockUnitOfWork.Verify(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockEventPublisher.Verify(x => x.PublishAsync(It.IsAny<CreateTaskItemEvent>()), Times.Once);
        _mockNotifier.Verify(x => x.SendNotificationAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);

    }

    [Fact]
    public async Task Handle_Should_Return_Validation_Error_And_Not_Save()
    {
        var payload = GetValidPayload();
        var command = new CreateTaskItemCommand { Payload = payload };

        _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                                .ReturnsAsync((Expression<Func<Team, bool>> filter) =>
                                {
                                    var compiled = filter.Compile();
                                    var team1 = new Team { Id = 5 };
                                    return compiled(team1);
                                });

        var response = await _handler.Handle(command, CancellationToken.None);

        Assert.False(response.IsSuccess);
        Assert.True(response.IsValidationFailure);

        _mockUnitOfWork.Verify(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockEventPublisher.Verify(x => x.PublishAsync(It.IsAny<CreateTaskItemEvent>()), Times.Never);
        _mockNotifier.Verify(x => x.SendNotificationAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.Never);
    }

    //[Fact]
    //public async Task Handle_Should_Roleback_When_AddAsync_Fails()
    //{
    //    var payload = GetValidPayload();
    //    var command = new CreateTaskItemCommand { Payload = payload };

    //    _mockUnitOfWork.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
    //    _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>())).Returns(Task.FromResult(true));
    //    _mockUnitOfWork.Setup(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>())).ThrowsAsync(new Exception("Database connection faield"));

    //    _mockUnitOfWork.Setup(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

    //    await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));

    //    _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    //    _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    //    _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    //}

    //[Fact]
    //public async Task Handle_Should_Roleback_When_SaveChangeAsync_Fails()
    //{
    //    var payload = GetValidPayload();
    //    var command = new CreateTaskItemCommand { Payload = payload };

    //    _mockUnitOfWork.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
    //    _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>())).Returns(Task.FromResult(true));
    //    _mockUnitOfWork.Setup(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

    //    _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Error from Database"));

    //    _mockUnitOfWork.Setup(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);


    //    await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));

    //    _mockUnitOfWork.Verify(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>()), Times.Once);
    //    _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    //    _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    //    _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

    //}

    //[Fact]
    //public async Task Handle_Should_Roleback_When_PublishAsync_Fails()
    //{
    //    var payload = GetValidPayload();
    //    var command = new CreateTaskItemCommand { Payload = payload };

    //    _mockUnitOfWork.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
    //    _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>())).Returns(Task.FromResult(true));
    //    _mockUnitOfWork.Setup(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);
    //    _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

    //    _mockEventPublisher.Setup(x => x.PublishAsync(It.IsAny<CreateTaskItemEvent>())).ThrowsAsync(new Exception("Error from Message Broker"));

    //    _mockUnitOfWork.Setup(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

    //    await Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(command, CancellationToken.None));

    //    _mockUnitOfWork.Verify(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>()), Times.Once);
    //    _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    //    _mockEventPublisher.Verify(x => x.PublishAsync(It.IsAny<CreateTaskItemEvent>()), Times.Once);
    //    _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    //    _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);

    //}

    [Fact]
    public async Task Handle_Should_Return_Success_When_NotificationService_Fails()
    {
        var payload = GetValidPayload();
        var command = new CreateTaskItemCommand { Payload = payload };

        _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>())).Returns(Task.FromResult(true));
        _mockUnitOfWork.Setup(x => x.TaskItemRepository.AddAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        _mockEventPublisher.Setup(x => x.PublishAsync(It.IsAny<CreateTaskItemEvent>())).Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        _mockNotifier.Setup(x => x.SendNotificationAsync(It.IsAny<Guid>(), It.IsAny<string>())).ThrowsAsync(new Exception("Error from Notification Services"));


        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

    }
}
