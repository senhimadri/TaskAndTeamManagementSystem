using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.AuthDto;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Auths.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public LoginDto Payload { get; set; } = default!;
}
