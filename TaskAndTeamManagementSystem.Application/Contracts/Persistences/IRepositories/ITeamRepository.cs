using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;

public interface ITeamRepository : IGenericRepository<Team, int>
{
    IQueryable<Team> TeamDetails(Expression<Func<Team, bool>> filter);

    IQueryable<Team> TeamDetails<TKey>(Expression<Func<Team, bool>> filter,
                                       Expression<Func<Team, TKey>>? orderBy = null,
                                       bool ascending = true);
}

