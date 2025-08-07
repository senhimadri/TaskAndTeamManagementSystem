using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;
using TaskAndTeamManagementSystem.Application.Extensions;
using TaskAndTeamManagementSystem.Contracts;
using TaskAndTeamManagementSystem.Shared.Results;


namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Update;

internal class UpdateTaskItemCommandHandler(IUnitOfWork unitofWork, IEventPublisher eventPublisher) : IRequestHandler<UpdateTaskItemCommand, Result>
{
    public async Task<Result> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateTaskItemPayloadDtoValidator(unitofWork)
                             .ValidateAsync(request.Payload);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var taskItem = await unitofWork.TaskItemRepository.GetByIdAsync(request.Id);

        if (taskItem is null)
            return Errors.TaskNotFound;

        request.Payload.MapToEntity(taskItem);

        unitofWork.TaskItemRepository.Update(taskItem);

        await unitofWork.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(new UpdateTaskItemEvent(taskItem.Id, taskItem.Title, taskItem.Description,
                                     taskItem.Status, taskItem.DueDate, taskItem.AssignedUserId));

        return Result.Success();
    }
}
