using Microsoft.AspNetCore.Identity;

namespace TaskAndTeamManagementSystem.Domain.Identities;

public class RoleClaim : IdentityRoleClaim<Guid>
{
    public ApplicationRole Role { get; set; } = default!;
}



