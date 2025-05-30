using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.AuthDtos.Validator;

public class UserRegistrationDtoValidator : AbstractValidator<IUserRegistrationDto>
{
    public UserRegistrationDtoValidator()
    {
        RuleFor(x => x.Name)
           .NotEmpty().NotNull().WithMessage("Name is required.")
           .MinimumLength(0).MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.UserName)
            .NotEmpty().NotNull().WithMessage("UserName is required.")
            .MaximumLength(200).WithMessage("User name cannot exit 200 characters");

        RuleFor(x => x.Email)
            .NotEmpty().NotNull().WithMessage("Name is required.")
            .EmailAddress();
    }
}
