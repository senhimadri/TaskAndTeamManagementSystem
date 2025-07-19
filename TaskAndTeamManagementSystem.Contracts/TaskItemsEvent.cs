using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Contracts;

public record CreateTaskItemEvent(long Id, string Title, string? Description, ActivityStatus Status, DateTimeOffset? DueDate, Guid AssignedUserId);
public record UpdateTaskItemEvent(long Id, string Title, string? Description, ActivityStatus Status, DateTimeOffset? DueDate, Guid AssignedUserId);
public record DeleteTaskItemEvent(long Id);
