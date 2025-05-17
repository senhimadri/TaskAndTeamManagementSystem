namespace TaskAndTeamManagementSystem.Domain;

public class Department : BaseDomain<int>
{
    public string Name { get; set; }=string.Empty;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
