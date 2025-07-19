using Moq;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Delete;
using TaskAndTeamManagementSystem.Contracts;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.UnitTest.Application.Features.Tests;

public class DeleteTaskItemCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IEventPublisher> _mockEventPublisher = new();

    private readonly DeleteTaskItemCommandHandler _handler;

    public DeleteTaskItemCommandHandlerTest()
    {
        _handler = new DeleteTaskItemCommandHandler(_mockUnitOfWork.Object, _mockEventPublisher.Object);
    }

    [Fact]
    public async Task Handle_ValidId_ShouldReturnSuccess()
    {
        var command = new DeleteTaskItemCommand { Id = 1 };

        var existingTaskItem = new TaskItem
        {
            Id = 1,
            Title = "Task to delete",
            Description = "Description",
            Status = ActivityStatus.ToDo,
            DueDate = DateTimeOffset.UtcNow.AddDays(1),
            AssignedUserId = Guid.NewGuid(),
            TeamId = 1
        };

        _mockUnitOfWork.Setup(x => x.TaskItemRepository.GetByIdAsync(command.Id)).ReturnsAsync(existingTaskItem);
        _mockUnitOfWork.Setup(x => x.TaskItemRepository.Delete(It.IsAny<TaskItem>()));
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        _mockEventPublisher.Setup(x => x.PublishAsync(It.IsAny<DeleteTaskItemEvent>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        _mockUnitOfWork.Verify(x => x.TaskItemRepository.Update(It.IsAny<TaskItem>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockEventPublisher.Verify(x => x.PublishAsync(It.IsAny<DeleteTaskItemEvent>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TaskNotFound_ShouldReturnError()
    {
        var command = new DeleteTaskItemCommand { Id = 1 };

        _mockUnitOfWork.Setup(x => x.TaskItemRepository.GetByIdAsync(command.Id)).ReturnsAsync((TaskItem?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equivalent(Errors.TaskNotFound, result.Error);

        _mockUnitOfWork.Verify(x => x.TaskItemRepository.Delete(It.IsAny<TaskItem>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockEventPublisher.Verify(x => x.PublishAsync(It.IsAny<DeleteTaskItemEvent>()), Times.Never);
    }
}
