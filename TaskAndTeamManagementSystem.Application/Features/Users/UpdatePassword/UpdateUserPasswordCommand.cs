using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Users.UpdatePassword;

public class UpdateUserPasswordCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public UpdateUserPasswordPayload Payload { get; set; } = default!;
}
