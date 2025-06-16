using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.AuthDtos.Validator;

public class CreateUserPayloadValidator : AbstractValidator<CreateUserPayload>
{
    public CreateUserPayloadValidator()
    {
        Include(x => new UserRegistrationDtoValidator());
        Include(x => new PasswordDtoValidator());
    }
}
