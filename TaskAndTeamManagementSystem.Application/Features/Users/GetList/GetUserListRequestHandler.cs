using MediatR;
using TaskAndTeamManagementSystem.Application.Contracts.Identities;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;
using TaskAndTeamManagementSystem.Application.Extensions;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Users.GetList;

public class GetUserListRequestHandler(IIdentityUnitOfWork unitOfWork) : IRequestHandler<GetUserListRequest, GetPaginationResponse<GetUserListResponse>>
{
    public async Task<GetPaginationResponse<GetUserListResponse>> Handle(GetUserListRequest request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.UserManager.Users.AsQueryable();

        query = request.Query.IsAscending ?  query.OrderBy(u => u.Id) : query.OrderByDescending(u => u.Id);


        if (!string.IsNullOrWhiteSpace(request.Query.SearchText))
        {
            query = query.Where(u => u.Name.Contains(request.Query.SearchText) ||
                                (u.PhoneNumber != null && u.PhoneNumber.Contains(request.Query.SearchText) )||
                                (u.Email != null && u.Email.Contains(request.Query.SearchText)));
        }

        var result = await query
                    .ToGetListResponse()
                    .ToPaginatedResultAsync(request.Query.PageNo, request.Query.PageSize);

        return result;
    }
}