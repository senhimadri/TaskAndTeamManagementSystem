using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace TaskAndTeamManagementSystem.Persistence.Repositories;

internal class TeamRepository(AppDbContext context) : GenericRepository<Team, int>(context), ITeamRepository
{
    private readonly AppDbContext _context = context;

    public IQueryable<Team> TeamDetails(Expression<Func<Team, bool>> filter)
    {
        return _context.Teams
            .Include(t => t.Tasks)
            .Where(filter);
    }

    public IQueryable<Team> TeamDetails<TKey>(Expression<Func<Team, bool>> filter,
                                             Expression<Func<Team, TKey>>? orderBy = null,
                                             bool ascending = true)
    {
        var query = _context.Teams.Where(filter);

        if (orderBy != null)
        {
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        return query.AsQueryable();
    }
}
