using System.Linq.Expressions;
using TaskNinjaHub.Application.Interfaces.Haves;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Bases.Interfaces;

public interface IBaseRepository<T> where T : class, IHaveId
{
    Task<T?> GetByIdAsync(int id);

    Task<IEnumerable<T>?> GetAllAsync();

    Task<IEnumerable<T>?> GetAllByPageAsync(int pageNumber, int pageSize);
        
    Task<IEnumerable<T>?> GetAllByFilterAsync(IDictionary<string, string?> filter);

    Task<IEnumerable<T>?> GetAllByFilterByPageAsync(IDictionary<string, string?> filter, int pageNumber, int pageSize);

    Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> expression);

    Task<OperationResult> AddAsync(T entity);

    Task<OperationResult> UpdateAsync(T entity);

    Task<OperationResult> RemoveAsync(T entity);
}