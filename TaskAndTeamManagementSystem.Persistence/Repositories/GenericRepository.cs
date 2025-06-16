using TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;
using TaskAndTeamManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TaskAndTeamManagementSystem.Persistence.Repositories;

public class GenericRepository<T, TKey>(AppDbContext dbContext) : IGenericRepository<T, TKey> where T : BaseDomain<TKey>
{
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);
    public void Add(T entity) => _dbSet.Add(entity);
    public void AddRange(IEnumerable<T> entities) => _dbSet.AddRange(entities);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
    public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

    public async Task<int> CountAsync(Expression<Func<T, bool>> filter) => await _dbSet.CountAsync(filter);

    public async Task<bool> IsAnyAsync(Expression<Func<T, bool>> filter) => await _dbSet.AnyAsync(filter);

    public async Task<IReadOnlyList<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> filter)
                                            => await _dbSet.AsNoTracking().Where(filter).ToListAsync();

    public async ValueTask<T?> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id);
}

