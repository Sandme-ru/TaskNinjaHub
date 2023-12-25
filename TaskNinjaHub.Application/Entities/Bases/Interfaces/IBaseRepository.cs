using System.Linq.Expressions;
using TaskNinjaHub.Application.Interfaces.Haves;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Bases.Interfaces;

/// <summary>
/// Interface IBaseRepository
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseRepository<T> where T : class, IHaveId
{
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;T&gt;.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>Task&lt;IQueryable&lt;T&gt;&gt;.</returns>
    Task<IEnumerable<T>?> GetAllAsync();
    
    /// <summary>
    /// Gets all by filter.
    /// </summary>
    /// <returns>Task&lt;IQueryable&lt;T&gt;&gt;.</returns>
    Task<IEnumerable<T>?> GetAllByFilterAsync(IDictionary<string, string?> filter);

    /// <summary>
    /// Finds the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;.</returns>
    Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>System.Threading.Tasks.Task.</returns>
    Task<OperationResult> AddAsync(T entity);

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>System.Threading.Tasks.Task.</returns>
    Task<OperationResult> UpdateAsync(T entity);

    /// <summary>
    /// Removes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>System.Threading.Tasks.Task.</returns>
    Task<OperationResult> RemoveAsync(T entity);
}