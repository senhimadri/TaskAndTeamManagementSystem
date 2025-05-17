namespace TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;

public interface ITaskItemDto
{
    public string Title { get; }
    public string? Description { get; }
    public int Status { get; }
    public DateTimeOffset? DueDate { get; }
    public Guid AssignedUserId { get; }
    public int TeamId { get; }
}
