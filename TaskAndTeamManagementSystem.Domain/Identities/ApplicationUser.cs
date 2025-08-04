using Microsoft.AspNetCore.Identity;

namespace TaskAndTeamManagementSystem.Domain.Identities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = default!;
    public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
    public ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();


    public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
}



