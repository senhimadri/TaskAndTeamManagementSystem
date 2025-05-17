using TaskAndTeamManagementSystem.Application.Common.Mappers;
using TaskAndTeamManagementSystem.Application.Common.QueryFilters.EmployeeQueryBuilders;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaskAndTeamManagementSystem.Application.Features.Employee.GetList;

public class GetListEmployeeRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListEmployeeRequest, List<GetEmployeesListResponse>>
{
    public async Task<List<GetEmployeesListResponse>> Handle(GetListEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employeeFilterBuilder = new EmployeeFilterBuilder();

        var employeeFilter = employeeFilterBuilder
                            .IncludeDepartmentIdList(request.DepartmentIds)
                            .IncludeDesignationIdList(request.DesignationIds)
                            .IncludeSearchText(request.SearchText)
                            .Build();

        var employeeList = await unitOfWork.EmployeeRepository
                        .GetEmployeesWithDetails(employeeFilter)
                        .ToGetListResponse()
                        .ToListAsync();

        return employeeList;
    }
}