using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Contracts;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Delete;

internal class DeleteTaskItemCommandHandler(IUnitOfWork unitofWork, IEventPublisher eventPublisher) :
                                                            IRequestHandler<DeleteTaskItemCommand, Result>
{
    public async Task<Result> Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var employee = await unitofWork.TaskItemRepository.GetByIdAsync(request.Id);

        if (employee is null)
            return Errors.TaskNotFound;

        unitofWork.TaskItemRepository.Delete(employee);

        await unitofWork.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(new DeleteTaskItemEvent(request.Id));

        return Result.Success();
    }
}

