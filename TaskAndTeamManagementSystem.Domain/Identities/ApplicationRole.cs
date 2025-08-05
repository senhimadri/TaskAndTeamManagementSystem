using Microsoft.AspNetCore.Identity;

namespace TaskAndTeamManagementSystem.Domain.Identities;

public class ApplicationRole : IdentityRole<Guid>
{
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
}



