using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.GetList;

public class GetTeamListRequest : IRequest<GetPaginationResponse<GetTeamsListResponse>>
{
    public TeamPaginationQuery Query { get; set; } = default!;
}