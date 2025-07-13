using MediatR;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.Delete;

public class DeleteTeamCommand : IRequest<Result>
{
    public int Id { get; set; }
}