using TaskAndTeamManagementSystem.Domain;
using System.Linq.Expressions;

namespace TaskAndTeamManagementSystem.Application.Common.QueryFilters.EmployeeQueryBuilders;

internal interface IEmployeeFilterBuilder
{
    IEmployeeFilterBuilder IncludeSearchText(string? searchText);   
    IEmployeeFilterBuilder IncludeDepartmentIdList(List<int>? departmentIdList);
    IEmployeeFilterBuilder IncludeDepartmentId(int departmentId);
    IEmployeeFilterBuilder IncludeDesignationIdList(List<int>? designationIdList);
    IEmployeeFilterBuilder IncludeDesignationId(int designationId);
    Expression<Func<Employee, bool>> Build();
}



