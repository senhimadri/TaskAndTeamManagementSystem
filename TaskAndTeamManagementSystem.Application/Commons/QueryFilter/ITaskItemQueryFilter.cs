using LinqKit;
using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Commons.QueryFilter;

public interface ITaskItemQueryFilter
{
    ITaskItemQueryFilter IncludeStatus(ActivityStatus? Id);
    ITaskItemQueryFilter IncludeAssignedTo(Guid? Id);
    ITaskItemQueryFilter IncludeCreatedby(Guid? Id);
    ITaskItemQueryFilter IncludeDueDate(DateTimeOffset? frondate, DateTimeOffset? todate);
    Expression<Func<TaskItem, bool>> Build();
}

public class TaskItemQueryFilter : ITaskItemQueryFilter
{
    private ExpressionStarter<TaskItem> filter;

    public TaskItemQueryFilter() => filter = PredicateBuilder.New<TaskItem>(true);

    public ITaskItemQueryFilter IncludeAssignedTo(Guid? Id)
    {
        filter = filter.And(x => Id == null || x.AssignedUserId == Id);
        return this;
    }

    public ITaskItemQueryFilter IncludeCreatedby(Guid? Id)
    {
        filter = filter.And(x => Id == null || x.CreatedByUserId == Id);
        return this;
    }

    public ITaskItemQueryFilter IncludeDueDate(DateTimeOffset? frondate, DateTimeOffset? todate)
    {
        filter = filter.And(x => (!frondate.HasValue || x.DueDate >= frondate)
                                && (!todate.HasValue || x.DueDate <= todate));
        return this;
    }

    public ITaskItemQueryFilter IncludeStatus(ActivityStatus? Id)
    {
        filter = filter.And(x => Id == 0 || x.Status == Id);
        return this;
    }

    public Expression<Func<TaskItem, bool>> Build() => filter;

}
