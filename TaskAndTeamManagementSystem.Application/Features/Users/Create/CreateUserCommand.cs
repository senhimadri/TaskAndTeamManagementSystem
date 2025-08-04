using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Users.Create;

public class CreateUserCommand : IRequest<Result<Guid>>
{
    public CreateUserPayload Payload { get; set; } = default!;
}
