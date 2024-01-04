using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Interfaces.Haves;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Bases.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IHaveId
{
    protected readonly DbContext? Context;

    protected BaseRepository(DbContext? context)
    {
        Context = context;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        if (Context != null)
            return await Context.Set<T>().FindAsync(id);
        return null;
    }

    public async Task<IEnumerable<T>?> GetAllAsync()
    {
        return await Context?.Set<T>().ToListAsync()!;
    }

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

    public Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> expression)
    {
        return Task.FromResult<IEnumerable<T>?>(Context?.Set<T>().Where(expression));
    }

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