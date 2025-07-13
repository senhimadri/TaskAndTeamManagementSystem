using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.GetById;

public class GetTeamByIdRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTeamByIdRequest, GetTeamByIdResponse?>
{
    public async Task<GetTeamByIdResponse?> Handle(GetTeamByIdRequest request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.TeamRepository.TeamDetails(x => x.Id == request.Id);
        var team = await query.ToGetByIdResponse().FirstOrDefaultAsync(cancellationToken);
        return team;
    }
}