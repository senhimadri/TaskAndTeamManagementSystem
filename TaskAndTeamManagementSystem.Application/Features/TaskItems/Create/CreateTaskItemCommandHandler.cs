using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;
using TaskAndTeamManagementSystem.Shared.Results;
using TaskAndTeamManagementSystem.Contracts;
using TaskAndTeamManagementSystem.Application.Extensions;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;

public class CreateTaskItemCommandHandler(IUnitOfWork _unitofWork, IRealTimeNotificationService _notifyer,
                            ICurrentUserService _currentUser, IEventPublisher _eventPublisher) 
                            : IRequestHandler<CreateTaskItemCommand, Result>
{
    public async Task<Result> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        await _unitofWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var validationResult = await new CreateTaskItemPayloadDtoValidator(_unitofWork)
                .ValidateAsync(request.Payload, cancellationToken);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationErrorList();
            }

            var taskItem = request.Payload.ToEntity(_currentUser.UserId);

            await _unitofWork.TaskItemRepository.AddAsync(taskItem);

            await _eventPublisher.PublishAsync(new CreateTaskItemEvent(
                taskItem.Id, taskItem.Title, taskItem.Description,
                taskItem.Status, taskItem.DueDate, taskItem.AssignedUserId));


            await _unitofWork.SaveChangesAsync(cancellationToken);
            await _unitofWork.CommitTransactionAsync(cancellationToken);

            await _notifyer.SendNotificationAsync(taskItem.AssignedUserId,
                $"You are assigned a task, Task Title: {taskItem.Title}");

            return Result.Success();
        }
        catch (Exception)
        {
            await _unitofWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
