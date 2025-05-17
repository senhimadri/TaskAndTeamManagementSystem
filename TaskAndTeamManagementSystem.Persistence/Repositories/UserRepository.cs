using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Persistence.Repositories;

internal class UserRepository(AppDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
{

}
