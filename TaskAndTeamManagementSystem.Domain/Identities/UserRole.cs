using Microsoft.AspNetCore.Identity;

namespace TaskAndTeamManagementSystem.Domain.Identities;

public class UserRole : IdentityUserRole<Guid>
{
    public ApplicationUser User { get; set; } = default!;
    public ApplicationRole Role { get; set; } = default!;
}



