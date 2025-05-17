namespace TaskAndTeamManagementSystem.Domain;

public class TaskItem : BaseDomain<long>
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public TaskStatus Status { get; set; }
    public DateTimeOffset DueDate { get; set; }

    public Guid AssignedUserId { get; set; }
    public User AssignedUser { get; set; } = default!;


    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = default!;

    public int TeamId { get; set; }
    public Team Team { get; set; } = default!;

}