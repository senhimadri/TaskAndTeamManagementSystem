using TaskAndTeamManagementSystem.Domain;
using System.Linq.Expressions;

namespace TaskAndTeamManagementSystem.Application.Contracts.Persistences.IRepositories;

public interface IGenericRepository<T, TKey> where T : class
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);

    void Update(T entity);

    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);

    Task<int> CountAsync(Expression<Func<T, bool>> filter);
    Task<bool> IsAnyAsync(Expression<Func<T, bool>> filter);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    ValueTask<T?> GetByIdAsync(TKey id);

}
