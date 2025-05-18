using LinqKit;
using System.Linq.Expressions;
using TaskAndTeamManagementSystem.Domain;
using TaskStatus = TaskAndTeamManagementSystem.Domain.TaskStatus;

namespace TaskAndTeamManagementSystem.Application.Commons.QueryFilter;

public interface ITaskItemQueryFilter
{
    ITaskItemQueryFilter IncludeStatus(TaskStatus? Id);
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
        filter = filter.And(x => Id == null ||  x.AssignedUserId == Id);
        return this;
    }

    public ITaskItemQueryFilter IncludeCreatedby(Guid? Id)
    {
        filter = filter.And(x =>Id == null || x.CreatedByUserId == Id);
        return this;
    }

    public ITaskItemQueryFilter IncludeDueDate(DateTimeOffset? frondate, DateTimeOffset? todate)
    {
        filter = filter.And(x => (frondate == null || x.DueDate >= frondate)
                                && (todate== null || x.DueDate <= todate));
        return this;
    }

    public ITaskItemQueryFilter IncludeStatus(TaskStatus? Id)
    {
        filter = filter.And(x =>Id == 0 || x.Status == Id);
        return this;
    }

    public Expression<Func<TaskItem, bool>> Build() => filter;

}
