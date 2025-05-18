using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;

public interface ITaskItemRepository : IGenericRepository<TaskItem, long>
{
    public IQueryable<TaskItem> TaskItemDetails<TKey>(Expression<Func<TaskItem, bool>> filter,
                                                Expression<Func<TaskItem, TKey>>? orderBy = null,
                                                bool ascending = true);
    public IQueryable<TaskItem> TaskItemDetails(Expression<Func<TaskItem, bool>> filter);
}

