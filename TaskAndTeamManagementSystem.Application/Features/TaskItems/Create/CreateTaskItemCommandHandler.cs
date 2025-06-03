using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;
using TaskAndTeamManagementSystem.Application.Helpers.Extensions;
using TaskAndTeamManagementSystem.Application.Helpers.Results;
using TaskAndTeamManagementSystem.Contracts.Events.TaskItems;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;

internal class CreateTaskItemCommandHandler(IUnitOfWork _unitofWork, IRealTimeNotificationService _notifyer,
                            ICurrentUserService _currentUser, IEventPublisher _eventPublisher) : IRequestHandler<CreateTaskItemCommand, Result>
{
    public async Task<Result> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateTaskItemPayloadDtoValidator(_unitofWork)
                             .ValidateAsync(request.Payload);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var taskItem = request.Payload.ToEntity(_currentUser.UserId);

        await _unitofWork.TaskItemRepository.AddAsync(taskItem);

        await _unitofWork.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new CreateTaskItemEvent(taskItem.Id,taskItem.Title,taskItem.Description,taskItem.Status,taskItem.DueDate,taskItem.AssignedUserId));

        await _notifyer.SendNotificationAsync(request.Payload.AssignedUserId,
                                    $"You are assigned a task , Task Title {request.Payload.Title}");

        return Result.Success();
    }
}
