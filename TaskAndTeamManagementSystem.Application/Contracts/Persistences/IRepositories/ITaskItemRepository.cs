using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;

public interface ITaskItemRepository : IGenericRepository<TaskItem, long>
{
    IQueryable<TaskItem> TaskItemDetails(Expression<Func<TaskItem, bool>> filter);
}

