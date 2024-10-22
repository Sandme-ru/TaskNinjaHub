﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Interfaces.Haves;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Bases.Repositories;

public abstract class BaseRepository<T>(DbContext context) : IBaseRepository<T> where T : class, IHaveId
{
    protected readonly DbContext Context = context;

    public async Task<T?> GetByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>?> GetAllAsync()
    {
        return await Context.Set<T>()
            .OrderByDescending(entity => entity.Id)
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<T>?> GetAllByPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
            return null;

        var queryable = Context.Set<T>()
            .OrderByDescending(entity => entity.Id)
            .AsQueryable();

        if (queryable == null)
            return null;

        var paginatedList = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return paginatedList;
    }
    
    public virtual async Task<IEnumerable<T>?> GetAllByFilterAsync(IDictionary<string, string?> filter)
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

        var filteredList = Context.Set<T>()
            .OrderByDescending(entity => entity.Id)
            .ToList()
            .Where(predicate.Compile());

        return filteredList;
    }

    public virtual async Task<IEnumerable<T>?> GetAllByFilterByPageAsync(IDictionary<string, string?> filter, int pageNumber, int pageSize)
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

        var queryable = Context.Set<T>()
            .OrderByDescending(entity => entity.Id)
            .ToList()
            .Where(predicate.Compile()); 

        var paginatedList = queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return paginatedList;
    }

    public Task<IEnumerable<T>?> FindAsync(Expression<Func<T, bool>> expression)
    {
        return Task.FromResult<IEnumerable<T>?>(Context.Set<T>().Where(expression));
    }

    public virtual async Task<OperationResult<T>> AddAsync(T entity)
    {
        try
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return OperationResult<T>.SuccessResult(entity);
        }
        catch (Exception ex)
        {
            return OperationResult<T>.FailedResult($"Failed to add entity: {ex.Message}");
        }
    }

    public virtual async Task<OperationResult<T>> UpdateAsync(T entity)
    {
        try
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync();
            return OperationResult<T>.SuccessResult(entity);
        }
        catch (Exception ex)
        {
            return OperationResult<T>.FailedResult($"Failed to update entity: {ex.Message}");
        }
    }

    public virtual async Task<OperationResult<T>> RemoveAsync(T entity)
    {
        try
        {
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
            return OperationResult<T>.SuccessResult();
        }
        catch (Exception ex)
        {
            return OperationResult<T>.FailedResult($"Failed to remove entity: {ex.Message}");
        }
    }
}