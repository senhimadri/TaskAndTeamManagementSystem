using Microsoft.AspNetCore.Identity;

namespace TaskAndTeamManagementSystem.Domain.Identities;

public class UserClaim : IdentityUserClaim<Guid>
{
    public ApplicationUser User { get; set; } = default!;
}



