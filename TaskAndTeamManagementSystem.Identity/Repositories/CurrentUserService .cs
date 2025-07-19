using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;

namespace TaskAndTeamManagementSystem.Identity.Repositories;

internal class CurrentUserService(IHttpContextAccessor _httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    private IReadOnlyList<string>? _roles;

    public Guid UserId => Guid.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var guid) ? guid : Guid.Empty;
    public string? UserName => User?.Identity?.Name;
    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

    public IReadOnlyList<string>? GetRoles()
    {
        if (_roles is not null)
        {
            return _roles;
        }
        _roles = User?.FindAll(ClaimTypes.Role)
            .Select(role => role.Value)
            .ToList();

        return _roles;
    }

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<Claim> Claims => User?.Claims ?? Enumerable.Empty<Claim>();

}
