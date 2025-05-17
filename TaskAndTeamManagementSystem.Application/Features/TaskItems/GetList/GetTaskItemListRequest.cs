using MediatR;
using TaskAndTeamManagementSystem.Application.Commons.QueryFilter;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.CommonDtos;
using TaskAndTeamManagementSystem.Application.Dtos.TaskItemDtos;

namespace TaskAndTeamManagementSystem.Application.Features.TaskItems.GetList;

public class GetTaskItemListRequest : IRequest<GetPaginationResponse<GetTaskItemsListResponse>>
{
    public TaskItemPaginationQuery Query { get; set; } = default!;
}

public class GetTaskItemListRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTaskItemListRequest, GetPaginationResponse<GetTaskItemsListResponse>>
{
    public Task<GetPaginationResponse<GetTaskItemsListResponse>> Handle(GetTaskItemListRequest request, CancellationToken cancellationToken)
    {

        ITaskItemQueryFilter taskItemFilterBuilder = new TaskItemQueryFilter();

        //var taskItemFilter = employeeFilterBuilder
        //                    .IncludeDepartmentIdList(request.DepartmentIds)
        //                    .IncludeDesignationIdList(request.DesignationIds)
        //                    .IncludeSearchText(request.SearchText)
        //                    .Build();

        //var employeeList = await unitOfWork.EmployeeRepository
        //                .GetEmployeesWithDetails(employeeFilter)
        //                .ToGetListResponse()
        //                .ToListAsync();

        return null;
    }
}

