using FluentValidation;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;

namespace TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;

internal class UpdateTaskItemPayloadDtoValidator : AbstractValidator<UpdateTaskItemPayload>
{
    public UpdateTaskItemPayloadDtoValidator(IUnitOfWork unitOfWork)
    {
        Include(new TaskItemDtoValidator(unitOfWork));
    }
}
