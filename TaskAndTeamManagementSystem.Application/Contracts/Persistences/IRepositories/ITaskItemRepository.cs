using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;

public interface ITaskItemRepository : IGenericRepository<TaskItem, long>;

