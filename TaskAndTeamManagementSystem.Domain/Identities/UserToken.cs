using Microsoft.AspNetCore.Identity;

namespace TaskAndTeamManagementSystem.Domain.Identities;

public class UserToken : IdentityUserToken<Guid>
{
    public ApplicationUser User { get; set; } = default!;
}



