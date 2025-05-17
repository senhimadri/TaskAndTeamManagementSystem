using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TaskAndTeamManagementSystem.Persistence.Repositories;

public class EmployeeRepository(AppDbContext context) : GenericRepository<Employee, Guid>(context), IEmployeeRepository
{
    private readonly AppDbContext _context = context;
    public IQueryable<Employee> GetEmployeesWithDetails(Expression<Func<Employee, bool>> filter)
    {
        var query = _context.Employees
                    .Include(e => e.Department)
                    .Include(e => e.Designation)
                    .Where(filter)
                    .AsQueryable();

        return query;

    }
}
