namespace TaskAndTeamManagementSystem.Contracts.Events.TaskItems;

public record CreateTaskItemEvent(long Id, string Title , string? Description, Domain.TaskStatus Status , DateTimeOffset? DueDate, Guid AssignedUserId);
public record UpdateTaskItemEvent(long Id, string Title, string? Description , Domain.TaskStatus Status , DateTimeOffset? DueDate, Guid AssignedUserId);
public record DeleteTaskItemEvent(long Id);
