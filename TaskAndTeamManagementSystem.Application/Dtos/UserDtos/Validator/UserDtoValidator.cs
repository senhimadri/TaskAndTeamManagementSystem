using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.UserDtos.Validator;

internal class UserDtoValidator : AbstractValidator<IUserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters")
            .When(x => x.PhoneNumber != null);
    }
}


internal class CreateUserPayloadValidator : AbstractValidator<CreateUserPayload>
{
    public CreateUserPayloadValidator()
    {
        Include(new UserDtoValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}

internal class UpdateUserPasswordPayloadValidator : AbstractValidator<UpdateUserPasswordPayload>
{
    public UpdateUserPasswordPayloadValidator()
    {
        RuleFor(x => x.CurrentPassword).SetValidator(new PasswordValidator());
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator());
    }
}

internal class UpdateUserPayloadValidator : AbstractValidator<UpdateUserPayload>
{
    public UpdateUserPayloadValidator()
    {
        Include(new UserDtoValidator());
    }
}
