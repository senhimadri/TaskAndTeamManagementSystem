using Moq;
using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Update;
using TaskAndTeamManagementSystem.Contracts;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.UnitTest.Application.Features.Tests;

public class UpdateTaskItemCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IEventPublisher> _eventPublisher = new();

    private readonly UpdateTaskItemCommandHandler _handler;

    public UpdateTaskItemCommandHandlerTest()
    {
        _handler = new UpdateTaskItemCommandHandler(_mockUnitOfWork.Object, _eventPublisher.Object);
    }

    private static UpdateTaskItemPayload GetValidPayload() => new UpdateTaskItemPayload
    (
        Title: "Updated Task",
        Description: "Updated description",
        Status: (int)ActivityStatus.InProgress,
        DueDate: DateTimeOffset.UtcNow.AddDays(3),
        AssignedUserId: Guid.NewGuid(),
        TeamId: 1
    );

    [Fact]
    public async Task Handle_ValidPayload_ShouldReturnSuccess()
    {
        var payload = GetValidPayload();
        var command = new UpdateTaskItemCommand { Id = 1, Payload = payload };

        var existingTaskItem = new TaskItem
        {
            Id = 1,
            Title = "Old Task",
            Description = "Old description",
            Status = ActivityStatus.ToDo,
            DueDate = DateTimeOffset.UtcNow.AddDays(1),
            AssignedUserId = Guid.NewGuid(),
            TeamId = 1
        };

        _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                                .ReturnsAsync((Expression<Func<Team, bool>> filter) =>
                                {
                                    var compiled = filter.Compile();
                                    var team1 = new Team { Id = 1 };
                                    return compiled(team1);
                                });

        _mockUnitOfWork.Setup(x => x.TaskItemRepository.GetByIdAsync(command.Id)).ReturnsAsync(existingTaskItem);
        _mockUnitOfWork.Setup(x => x.TaskItemRepository.Update(It.IsAny<TaskItem>()));
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        _eventPublisher.Setup(x => x.PublishAsync(It.IsAny<UpdateTaskItemEvent>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockUnitOfWork.Verify(x => x.TaskItemRepository.Update(It.IsAny<TaskItem>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _eventPublisher.Verify(x => x.PublishAsync(It.IsAny<UpdateTaskItemEvent>()), Times.Once);

    }

    [Fact]
    public async Task Handle_TaskNotFound_ShouldReturnError()
    {
        var payload = GetValidPayload();
        var command = new UpdateTaskItemCommand { Id = 1, Payload = payload };


        _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>())).ReturnsAsync(true);
        _mockUnitOfWork.Setup(x => x.TaskItemRepository.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((TaskItem?)null);


        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equivalent(Errors.TaskNotFound, result.Error);

        _mockUnitOfWork.Verify(x => x.TaskItemRepository.Update(It.IsAny<TaskItem>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _eventPublisher.Verify(x => x.PublishAsync(It.IsAny<UpdateTaskItemEvent>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidPayload_ShouldReturnValidationError()
    {
        var payload = new UpdateTaskItemPayload
        (
            Title: "", 
            Description: "desc",
            Status: (int)ActivityStatus.ToDo,
            DueDate: DateTimeOffset.UtcNow.AddDays(-1),
            AssignedUserId: Guid.NewGuid(),
            TeamId: 1
        );
        _mockUnitOfWork.Setup(x => x.TeamRepository.IsAnyAsync(It.IsAny<Expression<Func<Team, bool>>>()))
                        .ReturnsAsync((Expression<Func<Team, bool>> filter) =>
                        {
                            var compiled = filter.Compile();
                            var team1 = new Team { Id = 5 };
                            return compiled(team1);
                        });

        var command = new UpdateTaskItemCommand { Id = 1, Payload = payload };

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsValidationFailure);
        Assert.Equivalent(Errors.ValidationFailed, result.Error);
        Assert.False(result.ValidationErrors is null || !result.ValidationErrors.Any());
        Assert.NotEmpty(result.ValidationErrors);

        Assert.Collection(result.ValidationErrors,
            e => Assert.Equal(nameof(UpdateTaskItemPayload.Title), e.Key),
            e => Assert.Equal(nameof(UpdateTaskItemPayload.DueDate), e.Key),
            e => Assert.Equal(nameof(UpdateTaskItemPayload.TeamId), e.Key));


        _mockUnitOfWork.Verify(x => x.TaskItemRepository.GetByIdAsync(It.IsAny<long>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.TaskItemRepository.Update(It.IsAny<TaskItem>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _eventPublisher.Verify(x => x.PublishAsync(It.IsAny<UpdateTaskItemEvent>()), Times.Never);
    }
}