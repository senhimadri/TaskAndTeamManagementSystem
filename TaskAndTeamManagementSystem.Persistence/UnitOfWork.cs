using TaskAndTeamManagementSystem.Application.Contracts.Persistences;
using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace TaskAndTeamManagementSystem.Persistence
{
    internal class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;

        private bool _disposed = false;
        private bool _asyncDisposed = false;


        private IUserRepository? _userRepository;

        private ITaskItemRepository? _taskItemRepository;

        private ITeamRepository? _teamRepository;

        private IDbContextTransaction? _currentTransaction;

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public ITaskItemRepository TaskItemRepository => _taskItemRepository ??= new TaskItemRepository(_context);
        public ITeamRepository TeamRepository => _teamRepository??= new TeamRepository(_context);


        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
                                => await _context.SaveChangesAsync(cancellationToken);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
                return;

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                await _currentTransaction?.CommitAsync(cancellationToken)!;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _currentTransaction?.RollbackAsync(cancellationToken)!;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }


        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _currentTransaction?.Dispose();
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
            Dispose(false);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_asyncDisposed)
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }

                await _context.DisposeAsync();
                _asyncDisposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

    }
}
