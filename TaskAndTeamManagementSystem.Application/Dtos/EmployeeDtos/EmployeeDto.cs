namespace TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;

public record CreateEmployeePayload(string FirstName, string LastName, DateTimeOffset? DateOfBirth, int DepartmentId, int DesignationId) : IEmployeeDto;
public record UpdateEmployeePayload(string FirstName, string LastName, DateTimeOffset? DateOfBirth, int DepartmentId, int DesignationId) : IEmployeeDto;
public record GetEmployeeByIdResponse(string FirstName, string LastName, DateTimeOffset? DateOfBirth, int DepartmentId, string DepartmentName, 
                                        int DesignationId, string DesignationName);
public record GetEmployeesListResponse(string FirstName, string LastName, DateTimeOffset? DateOfBirth, string DepartmentName, string DesignationName);
public record EmployeePaginationQuery(List<int> DepartmentIds, List<int> DesignationIds, string? SearchText , int PageNo, int PageSize);


