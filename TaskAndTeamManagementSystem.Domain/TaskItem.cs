using TaskAndTeamManagementSystem.Domain.Bases;
using TaskAndTeamManagementSystem.Domain.Identities;

namespace TaskAndTeamManagementSystem.Domain;

public class TaskItem : BaseDomain<long>
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    public ActivityStatus Status { get; set; } = ActivityStatus.ToDo;
    public DateTimeOffset? DueDate { get; set; }

    public Guid AssignedUserId { get; set; }
    public ApplicationUser AssignedUser { get; set; } = default!;

    public Guid CreatedByUserId { get; set; }
    public ApplicationUser CreatedByUser { get; set; } = default!;

    public int? TeamId { get; set; }
    public Team? Team { get; set; }
}