using LinqKit;
using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Commons.QueryFilter;

public interface ITeamQueryFilter
{
    ITeamQueryFilter IncludeSearch(string? searchText);
    Expression<Func<Team, bool>> Build();
}
public class TeamQueryFilter : ITeamQueryFilter
{
    private ExpressionStarter<Team> filter = PredicateBuilder.New<Team>(true);

    public ITeamQueryFilter IncludeSearch(string? searchText)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
            filter = filter.And(x => x.Name.Contains(searchText));
        return this;
    }

    public Expression<Func<Team, bool>> Build() => filter;
}