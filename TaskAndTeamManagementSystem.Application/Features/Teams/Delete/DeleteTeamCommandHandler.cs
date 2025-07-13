using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Shared.Results;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.Delete;

public class DeleteTeamCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteTeamCommand, Result>
{
    public async Task<Result> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await unitOfWork.TeamRepository.GetByIdAsync(request.Id);
        if (team is null)
            return Errors.TeamNotFound;

        team.IsDelete = true;
        unitOfWork.TeamRepository.Update(team);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}