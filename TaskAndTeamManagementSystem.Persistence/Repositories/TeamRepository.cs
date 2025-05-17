using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Domain;

namespace TaskAndTeamManagementSystem.Persistence.Repositories;

internal class TeamRepository(AppDbContext context) : GenericRepository<Team, int>(context), ITeamRepository
{

}
