using TaskAndTeamManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace TaskAndTeamManagementSystem.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<Designation> Designations { get; set; } = default!;
    public DbSet<SalaryInformation> SalaryInformations { get; set; } = default!;
}
