using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;

namespace TaskAndTeamManagementSystem.Application.Contracts.Persistences;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
