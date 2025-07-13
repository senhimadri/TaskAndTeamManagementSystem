using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos.Validator;
using TaskAndTeamManagementSystem.Application.Extensions;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.Create;

public class CreateTeamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTeamCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new CreateTeamPayloadDtoValidator().ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var team = request.Payload.ToEntity();
        await unitOfWork.TeamRepository.AddAsync(team);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return team.Id;
    }
}