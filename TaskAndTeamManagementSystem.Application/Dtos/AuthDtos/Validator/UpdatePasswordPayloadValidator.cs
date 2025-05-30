using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.AuthDtos.Validator;

public class UpdatePasswordPayloadValidator : AbstractValidator<UpdatePasswordPayload>
{
    public UpdatePasswordPayloadValidator()
    {
        Include(x => new PasswordDtoValidator());
    }
}
