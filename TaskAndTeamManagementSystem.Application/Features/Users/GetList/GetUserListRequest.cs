using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;
using TaskAndTeamManagementSystem.Application.Dtos.UserDtos;

namespace TaskAndTeamManagementSystem.Application.Features.Users.GetList;

public class GetUserListRequest : IRequest<GetPaginationResponse<GetUserListResponse>>
{
    public UserPaginationQuery Query { get; set; } = default!;
}
