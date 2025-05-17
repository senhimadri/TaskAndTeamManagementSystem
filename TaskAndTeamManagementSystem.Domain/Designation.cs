namespace TaskAndTeamManagementSystem.Domain;

public class Designation : BaseDomain<int>
{
    public string Name { get; set; }=string.Empty ;
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
