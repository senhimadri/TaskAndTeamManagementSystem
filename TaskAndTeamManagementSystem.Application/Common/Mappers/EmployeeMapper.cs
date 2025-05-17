using TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;
using TaskAndTeamManagementSystem.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskAndTeamManagementSystem.Application.Common.Mappers;

public static class EmployeeMapper
{
    public static Employee ToEntity(this CreateEmployeePayload payload)
    {
        return new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = payload.FirstName,
            LastName = payload.LastName,
            DateOfBirth = payload.DateOfBirth,
            DepartmentId = payload.DepartmentId,
            DesignationId = payload.DesignationId
        };
    }

    public static void UpdateEntity(this Employee employee, UpdateEmployeePayload payload)
    {
        employee.FirstName = payload.FirstName;
        employee.LastName = payload.LastName;
        employee.DateOfBirth = payload.DateOfBirth;
        employee.DepartmentId = payload.DepartmentId;
        employee.DesignationId = payload.DesignationId;
    }

    public static IQueryable<GetEmployeeByIdResponse> ToGetByIdResponse(this IQueryable<Employee> query)
    {
        return query.Select(employee => new GetEmployeeByIdResponse(
                         employee.FirstName,
                         employee.LastName,
                         employee.DateOfBirth,
                         employee.DepartmentId,
                         employee.Department.Name,
                         employee.DesignationId,
                         employee.Designation.Name
                    ));
    }

    public static IQueryable<GetEmployeesListResponse> ToGetListResponse(this IQueryable<Employee> query)
    {
        return query.Select(employee => new GetEmployeesListResponse(
                     employee.FirstName,
                     employee.LastName,
                     employee.DateOfBirth,
                     employee.Department.Name,
                     employee.Designation.Name
                ));
    }
}
