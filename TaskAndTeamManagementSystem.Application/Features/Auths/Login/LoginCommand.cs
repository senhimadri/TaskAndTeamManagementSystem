using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDto;

namespace TaskAndTeamManagementSystem.Application.Features.Auths.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public LoginPayload Payload { get; set; } = default!;
}
