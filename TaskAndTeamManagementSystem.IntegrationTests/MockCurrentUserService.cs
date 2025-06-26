using System.Security.Claims;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;

namespace TaskAndTeamManagementSystem.IntegrationTests;

internal class MockCurrentUserService : ICurrentUserService
{
    private readonly Guid _userId;
    private readonly string? _userName;
    private readonly string? _email;
    private readonly IReadOnlyList<string> _roles;

    public MockCurrentUserService(Guid userId, string? userName, string? email, IReadOnlyList<string> roles)
    {
        _userId = userId;
        _userName = userName;
        _email = email;
        _roles = roles;
    }

    public Guid UserId => _userId;
    public string? UserName => _userName;
    public string? Email => _email;
    public IReadOnlyList<string> GetRoles() => _roles;
    public bool IsAuthenticated => _userId != Guid.Empty;
    public IEnumerable<Claim> Claims => new[]
    {
        new Claim(ClaimTypes.NameIdentifier, _userId.ToString()),
        new Claim(ClaimTypes.Name, _userName ?? ""),
        new Claim("email", _email ?? "")
    }.Concat(_roles.Select(r => new Claim(ClaimTypes.Role, r)));
}
