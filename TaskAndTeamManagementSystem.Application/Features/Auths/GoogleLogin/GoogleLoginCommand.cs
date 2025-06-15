using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDto;

namespace TaskAndTeamManagementSystem.Application.Features.Auths.GoogleLogin;

public class GoogleLoginCommand : IRequest<LoginResponse>
{
    public string Token { get; set; } = string.Empty;
}
