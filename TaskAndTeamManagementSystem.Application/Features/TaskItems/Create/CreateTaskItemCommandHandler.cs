using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;
using TaskAndTeamManagementSystem.Application.Helpers.Extensions;
using TaskAndTeamManagementSystem.Application.Helpers.Results;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.Create;

internal class CreateTaskItemCommandHandler(IUnitOfWork unitofWork) : IRequestHandler<CreateTaskItemCommand, Result>
{
    private readonly IUnitOfWork _unitofWork = unitofWork;

    public async Task<Result> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateTaskItemPayloadDtoValidator(_unitofWork)
                             .ValidateAsync(request.Payload);

        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var _createdBy = Guid.NewGuid();

        var taskItem = request.Payload.ToEntity(_createdBy);

        await _unitofWork.TaskItemRepository.AddAsync(taskItem);

        await _unitofWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
