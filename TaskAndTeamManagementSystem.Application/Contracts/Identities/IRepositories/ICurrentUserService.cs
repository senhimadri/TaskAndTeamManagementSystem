using System.Security.Claims;

namespace TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    IReadOnlyList<string> GetRoles();
    bool IsAuthenticated { get; }
    IEnumerable<Claim> Claims { get; }
}
