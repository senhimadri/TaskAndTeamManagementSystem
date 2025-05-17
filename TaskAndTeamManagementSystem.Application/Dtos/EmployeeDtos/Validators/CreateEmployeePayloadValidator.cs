using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos.Validators;

public class CreateEmployeePayloadValidator : AbstractValidator<CreateEmployeePayload>
{
    public CreateEmployeePayloadValidator()
    {
        Include(new EmployeeDtoValidator());
    }
}

