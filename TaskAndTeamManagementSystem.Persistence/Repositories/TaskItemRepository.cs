using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace TaskAndTeamManagementSystem.Persistence.Repositories;

internal class TaskItemRepository(AppDbContext context) : GenericRepository<TaskItem, long>(context), ITaskItemRepository
{
    private readonly AppDbContext _context = context;

    public IQueryable<TaskItem> TaskItemDetails(Expression<Func<TaskItem, bool>> filter,
                                                Expression<Func<TaskItem, object>>? orderBy = null,
                                                bool ascending = true)
    {
        var query = _context.TaskItems
            .Include(t => t.AssignedUser)
            .Include(t => t.CreatedByUser)
            .Include(t => t.Team)
            .Where(filter);

        if (orderBy != null)
        {
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        return query.AsQueryable();
    }
}
