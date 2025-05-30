using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.AuthDtos.Validator;

public class UpdateUserPayloadValidator : AbstractValidator<UpdateUserPayload>
{
    public UpdateUserPayloadValidator()
    {
        Include(x => new UserRegistrationDtoValidator());
    }
}
