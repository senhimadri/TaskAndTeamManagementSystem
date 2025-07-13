using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos.Validator;
using TaskAndTeamManagementSystem.Application.Extensions;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.Update;

public class UpdateTeamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateTeamCommand, Result>
{
    public async Task<Result> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await new UpdateTeamPayloadDtoValidator().ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorList();

        var team = await unitOfWork.TeamRepository.GetByIdAsync(request.Id);
        if (team is null)
            return Errors.NewError(404, "Team not found");

        request.Payload.MapToEntity(team);
        unitOfWork.TeamRepository.Update(team);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}