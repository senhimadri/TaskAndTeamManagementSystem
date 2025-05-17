namespace TaskAndTeamManagementSystem.Domain;

public class SalaryInformation : BaseDomain<long>
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public decimal Amount { get; set; }
    public bool IsCurrent { get; set; }

}
