using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;
using TaskAndTeamManagementSystem.Application.Helpers.Extensions;
using TaskAndTeamManagementSystem.Application.Helpers.Results;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;


namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Update;

public class UpdateTaskItemCommand : IRequest<Result>
{
    public long Id { get; set; }
    public UpdateTaskItemPayload Payload { get; set; } = default!;
}

internal class UpdateTaskItemCommandHandler(IUnitOfWork unitofWork) : IRequestHandler<UpdateTaskItemCommand, Result>
{
    private readonly IUnitOfWork _unitofWork = unitofWork;

    public async Task<Result> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateTaskItemPayloadDtoValidator(_unitofWork)
                             .ValidateAsync(request.Payload);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var taskItem = await _unitofWork.TaskItemRepository.GetByIdAsync(request.Id);


        if (taskItem is null)
            return Errors.TaskNotFound;

        request.Payload.MapToEntity(taskItem);

        _unitofWork.TaskItemRepository.Update(taskItem);

        await _unitofWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
