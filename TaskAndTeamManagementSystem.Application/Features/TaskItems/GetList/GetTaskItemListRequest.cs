using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.GetList;

public class GetTaskItemListRequest : IRequest<GetPaginationResponse<GetTaskItemsListResponse>>
{
    public TaskItemPaginationQuery Query { get; set; } = default!;
}

