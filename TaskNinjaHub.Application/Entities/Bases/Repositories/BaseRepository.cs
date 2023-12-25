using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Interfaces.Haves;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Bases.Repositories;

/// <summary>
/// Class BaseRepository.
/// Implements the <see cref="IBaseRepository{T}" />
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="IBaseRepository{T}" />
public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IHaveId
{
    /// <summary>
    /// The context
    /// </summary>
    protected readonly DbContext? Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    protected BaseRepository(DbContext? context)
    {
        Context = context;
    }

    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;T&gt;.</returns>
    public async Task<T?> GetByIdAsync(int id)
    {
        if (Context != null)
            return await Context.Set<T>().FindAsync(id);
        return null;
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>Task&lt;IQueryable&lt;T&gt;&gt;.</returns>
    public async Task<IEnumerable<T>?> GetAllAsync()
    {
        return await Context?.Set<T>().ToListAsync()!;
    }

    /// <summary>
    /// Gets all by filter.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <returns>Task&lt;IQueryable&lt;T&gt;&gt;.</returns>
    public async Task<IEnumerable<T>?> GetAllByFilterAsync(IDictionary<string, string?> filter)
    {
        Expression<Func<T, bool>> predicate = _ => true;

        foreach (var property in filter)
        {
            switch (property.Value)
            {
                case null:
                    continue;
                case "0":
                    continue;
                case var propertyValueString when string.IsNullOrEmpty(propertyValueString):
                    continue;
                default:
                {
                    Expression<Func<T, bool>> filterExpression =
                        entity => string.Equals(entity
                            .GetType()
                            .GetProperties()
                            .First(p => Equals(p.Name.Trim(), property.Key.Trim()))
                            .GetValue(entity)!
                            .ToString(), property.Value, StringComparison.Ordinal);
                    predicate = Expression.Lambda<Func<T, bool>>(
                        Expression.AndAlso(predicate.Body,
                            Expression.Invoke(filterExpression, predicate.Parameters)), predicate.Parameters[0]);
                    break;
                }
            }
        }

        return Context?.Set<T>().ToList().Where(predicate.Compile());
    }

    /// <summary>
    /// Finds the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;.</returns>
    public Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> expression)
    {
        return Task.FromResult<IEnumerable<T>?>(Context?.Set<T>().Where(expression));
    }

    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>System.Threading.Tasks.Task.</returns>
    public async Task<OperationResult> AddAsync(T entity)
    {
        try
        {
            await (Context?.Set<T>()!).AddAsync(entity);
            await Context?.SaveChangesAsync()!;
            return OperationResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return OperationResult.FailedResult($"Failed to add entity: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>System.Threading.Tasks.Task.</returns>
    public async Task<OperationResult> UpdateAsync(T entity)
    {
        try
        {
            Context?.Set<T>().Update(entity);
            await Context?.SaveChangesAsync()!;
            return OperationResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return OperationResult.FailedResult($"Failed to update entity: {ex.Message}");
        }
    }

    /// <summary>
    /// Removes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>System.Threading.Tasks.Task.</returns>
    public async Task<OperationResult> RemoveAsync(T entity)
    {
        try
        {
            Context?.Set<T>().Remove(entity);
            await Context?.SaveChangesAsync()!;
            return OperationResult.SuccessResult();
        }
        catch (Exception ex)
        {
            return OperationResult.FailedResult($"Failed to remove entity: {ex.Message}");
        }
    }
}