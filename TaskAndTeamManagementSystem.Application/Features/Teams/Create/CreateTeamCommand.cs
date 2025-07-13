using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.Create;

public class CreateTeamCommand : IRequest<Result<int>>
{
    public CreateTeamPayload Payload { get; set; } = default!;
}