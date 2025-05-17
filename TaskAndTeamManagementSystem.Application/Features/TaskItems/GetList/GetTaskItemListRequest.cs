using MediatR;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.GetList;

public class GetTaskItemListRequest : IRequest<GetPaginationResponse<GetTaskItemsListResponse>>
{
    public TaskItemPaginationQuery Query { get; set; } = default!;
}

public class GetTaskItemListRequestHandler : IRequestHandler<GetTaskItemListRequest, GetPaginationResponse<GetTaskItemsListResponse>>
{
    public Task<GetPaginationResponse<GetTaskItemsListResponse>> Handle(GetTaskItemListRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

