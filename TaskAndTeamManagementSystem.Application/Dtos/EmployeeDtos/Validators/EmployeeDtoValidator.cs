using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos.Validators;

public class EmployeeDtoValidator : AbstractValidator<IEmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Length(2, 50)
            .WithMessage("First name must be between 2 and 50 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .Length(2, 50)
            .WithMessage("Last name must be between 2 and 50 characters.");
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of birth is required.")
            .LessThan(DateTimeOffset.Now)
            .WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department ID is required.")
            .GreaterThan(0)
            .WithMessage("Department ID must be greater than 0.");
        RuleFor(x => x.DesignationId)
            .NotEmpty()
            .WithMessage("Designation ID is required.")
            .GreaterThan(0)
            .WithMessage("Designation ID must be greater than 0.");
    }
}

