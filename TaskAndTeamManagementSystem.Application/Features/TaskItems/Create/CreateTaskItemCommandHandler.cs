using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Notifications;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;
using TaskAndTeamManagementSystem.Application.Helpers.Extensions;
using TaskAndTeamManagementSystem.Application.Helpers.Results;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;

internal class CreateTaskItemCommandHandler(IUnitOfWork unitofWork, IRealTimeNotificationService notifyer) : IRequestHandler<CreateTaskItemCommand, Result>
{
    private readonly IUnitOfWork _unitofWork = unitofWork;
    private readonly IRealTimeNotificationService _notifyer = notifyer;

    public async Task<Result> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateTaskItemPayloadDtoValidator(_unitofWork)
                             .ValidateAsync(request.Payload);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var createdBy = Guid.Parse("FA0F1035-19F7-49F6-8AD9-08DD9FFF3137");

        var taskItem = request.Payload.ToEntity(createdBy);

        await _unitofWork.TaskItemRepository.AddAsync(taskItem);

        await _unitofWork.SaveChangesAsync(cancellationToken);

        await _notifyer.SendNotificationAsync(request.Payload.AssignedUserId,
                                    $"You are assigned a task , Task Title {request.Payload.Title}");

        return Result.Success();
    }
}
