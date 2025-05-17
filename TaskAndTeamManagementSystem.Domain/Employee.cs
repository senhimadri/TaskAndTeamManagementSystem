namespace TaskAndTeamManagementSystem.Domain;

public class Employee: BaseDomain<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; }= string.Empty;
    public DateTimeOffset? DateOfBirth { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }= default!;

    public int DesignationId { get; set; }
    public Designation Designation { get; set; } = default!;

    public ICollection<SalaryInformation> SalaryInformations { get; set; } = new List<SalaryInformation>();

}
