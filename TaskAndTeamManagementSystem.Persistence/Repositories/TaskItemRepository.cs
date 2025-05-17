using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Persistence.Repositories;

internal class TaskItemRepository(AppDbContext context) : GenericRepository<TaskItem, long>(context), ITaskItemRepository
{
}
