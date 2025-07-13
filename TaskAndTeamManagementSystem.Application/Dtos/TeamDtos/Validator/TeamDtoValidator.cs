using FluentValidation;

namespace TaskAndTeamManagementSystem.Application.Dtos.TeamDtos.Validator;

public class TeamDtoValidator : AbstractValidator<ITeamDto>
{
    public TeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MaximumLength(200).WithMessage("Team name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
    }
}

public class CreateTeamPayloadDtoValidator : AbstractValidator<CreateTeamPayload>
{
    public CreateTeamPayloadDtoValidator()
    {
        Include(new TeamDtoValidator());
    }
}
public class UpdateTeamPayloadDtoValidator : AbstractValidator<UpdateTeamPayload>
{
    public UpdateTeamPayloadDtoValidator()
    {
        Include(new TeamDtoValidator());
    }
}