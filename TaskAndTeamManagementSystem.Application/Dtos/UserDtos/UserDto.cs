namespace TaskAndTeamManagementSystem.Application.Dtos.UserDtos;

public record CreateUserPayload(string Name, string Email): IUserDto;
public record UpdateUserPayload(string Name, string Email): IUserDto;
public record GetUserIdResponse(string Name, string Email);
public record GetEmployeesListResponse(Guid Id,string Name, string Email);
public record EmployeePaginationQuery(List<int> DepartmentIds, List<int> DesignationIds, string? SearchText, int PageNo, int PageSize);


public interface IUserDto
{
    public string Name { get;  }
    public string Email { get; }
}