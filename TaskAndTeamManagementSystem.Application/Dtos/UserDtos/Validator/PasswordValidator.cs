using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.UserDtos.Validator;

internal class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator(bool requireSpecialCharacters = true)
    {
        RuleFor(password => password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters");

        When(_ => requireSpecialCharacters, () =>
        {
            RuleFor(password => password)
                .Matches(@"[!@#$%^&*]").WithMessage("Password must contain at least one special character");
        });
    }
}
