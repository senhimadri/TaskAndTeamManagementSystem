using MediatR;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Delete;

public class DeleteTaskItemCommand : IRequest<Result>
{
    public long Id { get; set; }
}

