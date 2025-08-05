using Microsoft.AspNetCore.Identity;

namespace TaskAndTeamManagementSystem.Domain.Identities;

public class UserLogin : IdentityUserLogin<Guid>
{
    public ApplicationUser User { get; set; } = default!;
}



