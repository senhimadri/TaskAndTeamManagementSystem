using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.LoginDtos;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Auths.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public LoginPayload Payload { get; set; } = default!;
}
