using TaskAndTeamManagementSystem.Domain;
using System.Linq.Expressions;

namespace TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;

public interface IEmployeeRepository : IGenericRepository<Employee,Guid>
{
    IQueryable<Employee> GetEmployeesWithDetails(Expression<Func<Employee, bool>> filter);
}
