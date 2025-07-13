using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.Update;

public class UpdateTeamCommand : IRequest<Result>
{
    public int Id { get; set; }
    public UpdateTeamPayload Payload { get; set; } = default!;
}