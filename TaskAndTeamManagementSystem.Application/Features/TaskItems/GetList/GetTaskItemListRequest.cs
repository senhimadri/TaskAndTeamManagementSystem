using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskAndTeamManagementSystem.Application.Commons.Mappers;
using TaskAndTeamManagementSystem.Application.Commons.QueryFilter;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;
using TaskAndTeamManagementSystem.Application.Helpers.Extensions;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.GetList;

public class GetTaskItemListRequest : IRequest<GetPaginationResponse<GetTaskItemsListResponse>>
{
    public TaskItemPaginationQuery Query { get; set; } = default!;
}

public class GetTaskItemListRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTaskItemListRequest, GetPaginationResponse<GetTaskItemsListResponse>>
{
    public async Task<GetPaginationResponse<GetTaskItemsListResponse>> Handle(GetTaskItemListRequest request, CancellationToken cancellationToken)
    {

        ITaskItemQueryFilter taskItemFilterBuilder = new TaskItemQueryFilter();

        var taskItemFilter = taskItemFilterBuilder
                            .IncludeStatus(request.Query.StatusId)
                            .IncludeAssignedTo(request.Query.AssignedUserId)
                            .IncludeCreatedby(request.Query.CreatedById)
                            .IncludeDueDate(request.Query.FromDate , request.Query.ToDate)
                            .Build();

        var employeeList = await unitOfWork.TaskItemRepository
                        .TaskItemDetails(taskItemFilter)
                        .ToGetListResponse()
                        .ToPaginatedResultAsync(request.Query.PageNo, request.Query.PageSize);

        return employeeList;
    }
}

