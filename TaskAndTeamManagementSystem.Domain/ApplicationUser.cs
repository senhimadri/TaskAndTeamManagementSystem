using Microsoft.AspNetCore.Identity;

namespace TaskAndTeamManagementSystem.Domain;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = default!;
    public string RefreshToken { get; set; } = string.Empty;
    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
}

public class ApplicationRole : IdentityRole<Guid>;


