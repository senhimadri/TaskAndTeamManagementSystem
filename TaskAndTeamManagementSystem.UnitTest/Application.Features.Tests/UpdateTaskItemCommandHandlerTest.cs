using Moq;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Application.Features.TaskItems.Update;
using TaskAndTeamManagementSystem.Domain;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.UnitTest.Application.Features.Tests;

public class UpdateTaskItemCommandHandlerTest
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<ITaskItemRepository> _mockTaskItemRepository = new();

    private readonly UpdateTaskItemCommandHandler _handler;

    public UpdateTaskItemCommandHandlerTest()
    {
        _mockUnitOfWork.Setup(x => x.TaskItemRepository).Returns(_mockTaskItemRepository.Object);
        _handler = new UpdateTaskItemCommandHandler(_mockUnitOfWork.Object);
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

        _mockTaskItemRepository.Setup(x => x.GetByIdAsync(command.Id)).ReturnsAsync(existingTaskItem);
        _mockTaskItemRepository.Setup(x => x.Update(It.IsAny<TaskItem>()));
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        _mockTaskItemRepository.Verify(x => x.Update(It.IsAny<TaskItem>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TaskNotFound_ShouldReturnError()
    {
        var payload = GetValidPayload();
        var command = new UpdateTaskItemCommand { Id = 99, Payload = payload };

        _mockTaskItemRepository.Setup(x => x.GetByIdAsync(command.Id)).ReturnsAsync((TaskItem?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(Errors.TaskNotFound, result.Error);
        _mockTaskItemRepository.Verify(x => x.Update(It.IsAny<TaskItem>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidPayload_ShouldReturnValidationError()
    {
        // Simulate invalid payload by using a payload that fails validation
        var payload = new UpdateTaskItemPayload
        (
            Title: "", // Invalid: empty title
            Description: "desc",
            Status: (int)ActivityStatus.ToDo,
            DueDate: DateTimeOffset.UtcNow.AddDays(1),
            AssignedUserId: Guid.NewGuid(),
            TeamId: 1
        );
        var command = new UpdateTaskItemCommand { Id = 1, Payload = payload };

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsValidationFailure);
        _mockTaskItemRepository.Verify(x => x.Update(It.IsAny<TaskItem>()), Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}