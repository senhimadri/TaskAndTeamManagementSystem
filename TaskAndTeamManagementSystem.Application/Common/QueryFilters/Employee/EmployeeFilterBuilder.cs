using TaskAndTeamManagementSystem.Domain;
using System.Linq.Expressions;
using LinqKit;

namespace TaskAndTeamManagementSystem.Application.Common.QueryFilters.EmployeeQueryBuilders;

internal class EmployeeFilterBuilder : IEmployeeFilterBuilder
{
    private ExpressionStarter<Employee> filter;

    public EmployeeFilterBuilder() => filter = PredicateBuilder.New<Employee>(true);

    public IEmployeeFilterBuilder IncludeSearchText(string? searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
            filter = filter.And(x =>
               (x.FirstName != null && x.FirstName.Contains(searchText)) ||
               (x.LastName != null && x.LastName.Contains(searchText)) ||
               (x.Department != null && x.Department.Name.Contains(searchText)) ||
               (x.Designation != null && x.Designation.Name.Contains(searchText)));

        return this;
    }

    public IEmployeeFilterBuilder IncludeDepartmentIdList(List<int>? departmentIdList)
    {
        if (departmentIdList?.Any() == true)
            filter = filter.And(x => departmentIdList.Contains(x.DepartmentId));
        return this;
    }

    public IEmployeeFilterBuilder IncludeDepartmentId(int departmentId)
    {
        filter = filter.And(x => x.DepartmentId == departmentId);
        return this;
    }

    public IEmployeeFilterBuilder IncludeDesignationIdList(List<int>? designationIdList)
    {
        if (designationIdList?.Any()== true)
            filter = filter.And(x => designationIdList.Contains(x.DesignationId));
        return this;
    }

    public IEmployeeFilterBuilder IncludeDesignationId(int designationId)
    {
        filter = filter.And(x => x.DesignationId == designationId);
        return this;
    }

    public Expression<Func<Employee, bool>> Build() => filter;
}



