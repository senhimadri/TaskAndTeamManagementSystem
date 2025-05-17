using MediatR;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Delete;

public class DeleteTaskItemCommand : IRequest<Result>
{
    public long Id { get; set; }
}


internal class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand, Result>
{
    public Task<Result> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

