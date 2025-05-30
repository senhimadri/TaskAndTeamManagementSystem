using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.AuthDtos.Validator;

public class PasswordDtoValidator : AbstractValidator<IPasswordDto>
{
    public PasswordDtoValidator() 
    {
        RuleFor(x => x.Password)
            .NotEmpty().NotNull().WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]")
            .WithMessage("Password must contain at least one digit.")
            .Matches(@"[\W_]")
            .WithMessage("Password must contain at least one special character.");
    }
}
