﻿using MediatR;
using Serilog;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Identities.IRepositories;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.MessageBrokers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;
using TaskAndTeamManagementSystem.Application.Extensions;
using TaskAndTeamManagementSystem.Contracts;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;

public class CreateTaskItemCommandHandler(IUnitOfWork unitofWork, IRealTimeNotificationService notifyer,
                            ICurrentUserService currentUser, IEventPublisher eventPublisher)
                            : IRequestHandler<CreateTaskItemCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateTaskItemPayloadDtoValidator(unitofWork)
                .ValidateAsync(request.Payload, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToValidationErrorList();
        }
        var taskItem = request.Payload.ToEntity(currentUser.UserId);

        await unitofWork.TaskItemRepository.AddAsync(taskItem);

        await unitofWork.SaveChangesAsync(cancellationToken);

        await eventPublisher.PublishAsync(new CreateTaskItemEvent(
            taskItem.Id, taskItem.Title, taskItem.Description,
            taskItem.Status, taskItem.DueDate, taskItem.AssignedUserId));


        try
        {
            await notifyer.SendNotificationAsync(taskItem.AssignedUserId,
                                $"You are assigned a task, Task Title: {taskItem.Title}");
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Notification failed for user {UserId}", request.Payload.AssignedUserId);
        }

        return taskItem.Id;
    }
}
