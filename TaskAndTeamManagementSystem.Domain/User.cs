namespace TaskAndTeamManagementSystem.Domain;

public class User : BaseDomain<Guid>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();

}
