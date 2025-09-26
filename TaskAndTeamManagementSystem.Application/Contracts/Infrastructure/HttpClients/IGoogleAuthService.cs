using TaskAndTeamManagementSystem.Application.Dtos.AuthDtos;

namespace TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.HttpClients;

public interface IGoogleAuthService
{
    Task<GoogleUserInfoDto> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken = default);

}
