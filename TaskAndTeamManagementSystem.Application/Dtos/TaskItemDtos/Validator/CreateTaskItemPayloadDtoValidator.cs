using FluentValidation;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;

namespace TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos.Validator;

public class CreateTaskItemPayloadDtoValidator : AbstractValidator<CreateTaskItemPayload>
{
    public CreateTaskItemPayloadDtoValidator(IUnitOfWork unitOfWork)
    {
        Include(new TaskItemDtoValidator(unitOfWork));
    }
}
