using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Commons.QueryFilter;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;
using TaskAndTeamManagementSystem.Application.Dtos.TeamDtos;
using TaskAndTeamManagementSystem.Application.Extensions;

namespace TaskAndTeamManagementSystem.Application.Features.Teams.GetList;

public class GetTeamListRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTeamListRequest, GetPaginationResponse<GetTeamsListResponse>>
{
    public async Task<GetPaginationResponse<GetTeamsListResponse>> Handle(GetTeamListRequest request, CancellationToken cancellationToken)
    {
        ITeamQueryFilter filterBuilder = new TeamQueryFilter();
        var filter = filterBuilder.IncludeSearch(request.Query.SearchText).Build();

        var teamList = await unitOfWork.TeamRepository
                    .TeamDetails(filter: filter, orderBy: x => x.Id, request.Query.IsAscending)
                    .ToGetListResponse()
                    .ToPaginatedResultAsync(request.Query.PageNo, request.Query.PageSize);

        return teamList;
    }
}