using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Users.Update;

public class UpdateUserCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public UpdateUserPayload Payload { get; set; } = default!;
}
