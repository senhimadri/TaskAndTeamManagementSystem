using Microsoft.AspNetCore.Identity;
using TaskAndTeamManagementSystem.Domain.Identities;

namespace TaskAndTeamManagementSystem.Application.Contracts.Identities;

public interface IIdentityUnitOfWork : IDisposable
{
    UserManager<ApplicationUser> UserManager { get; }
    SignInManager<ApplicationUser> SignInManager { get; }
    RoleManager<ApplicationRole> RoleManager { get; }
    Task<int> SaveChangesAsync();
}
