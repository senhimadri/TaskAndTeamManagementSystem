namespace TaskAndTeamManagementSystem.Application.Dtos.EmployeeDtos;

public interface IEmployeeDto
{
    public string FirstName { get; }
    public string LastName { get; }
    public DateTimeOffset? DateOfBirth { get; }
    public int DepartmentId { get; }
    public int DesignationId { get; }
}

