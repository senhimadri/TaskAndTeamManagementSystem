using Microsoft.AspNetCore.Identity;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;

public interface ITokenUtils
{
    string GenerateAccessToken(ApplicationUser user, IList<string>? roles);
    string GenerateRefreshToken(Guid userId);
}
