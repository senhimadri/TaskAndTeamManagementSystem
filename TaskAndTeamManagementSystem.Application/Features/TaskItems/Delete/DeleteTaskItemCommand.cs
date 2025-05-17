using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Delete;

public class DeleteTaskItemCommand : IRequest<Result>
{
    public long Id { get; set; }
}


internal class DeleteTaskItemCommandHandler(IUnitOfWork unitofWork) : IRequestHandler<DeleteTaskItemCommand, Result>
{
    private readonly IUnitOfWork _unitofWork = unitofWork;

    public async Task<Result> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var employee = await _unitofWork.TaskItemRepository.GetByIdAsync(request.Id);

        if (employee is null)
            return Errors.TaskNotFound;

        employee.IsDelete = true;

        _unitofWork.TaskItemRepository.Update(employee);

        await _unitofWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

