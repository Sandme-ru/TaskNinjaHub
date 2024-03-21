using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Tasks.Repositories;

public class TaskRepository(ITaskNinjaHubDbContext context) : BaseRepository<CatalogTask>((DbContext)context), ITaskRepository
{
    public override async Task<IEnumerable<CatalogTask>?> GetAllByPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
            return null;

        var queryable = Context.Set<CatalogTask>()
            .OrderByDescending(task => task.DateCreated)
            .Where(task => task.OriginalTaskId == null)
            .AsQueryable();

        if (queryable == null)
            return null;

        var paginatedList = await queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return paginatedList;
    }

    public override async Task<IEnumerable<CatalogTask>?> GetAllByFilterAsync(IDictionary<string, string?> filter)
    {
        Expression<Func<CatalogTask, bool>> predicate = _ => true;

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
                    Expression<Func<CatalogTask, bool>> filterExpression =
                        entity => string.Equals(entity
                            .GetType()
                            .GetProperties()
                            .First(p => Equals(p.Name.Trim(), property.Key.Trim()))
                            .GetValue(entity)!
                            .ToString(), property.Value, StringComparison.Ordinal);

                    predicate = Expression.Lambda<Func<CatalogTask, bool>>(
                        Expression.AndAlso(predicate.Body,
                            Expression.Invoke(filterExpression, predicate.Parameters)), predicate.Parameters[0]);
                    break;
                }
            }
        }

        var filteredList = Context.Set<CatalogTask>()
            .Where(task => task.OriginalTaskId == null)
            .OrderByDescending(task => task.Id)
            .ToList()
            .Where(predicate.Compile());

        return filteredList;
    }

    public override async Task<IEnumerable<CatalogTask>?> GetAllByFilterByPageAsync(IDictionary<string, string?> filter, int pageNumber, int pageSize)
    {
        Expression<Func<CatalogTask, bool>> predicate = _ => true;

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
                    Expression<Func<CatalogTask, bool>> filterExpression =
                        entity => string.Equals(entity
                            .GetType()
                            .GetProperties()
                            .First(p => Equals(p.Name.Trim(), property.Key.Trim()))
                            .GetValue(entity)!
                            .ToString(), property.Value, StringComparison.Ordinal);

                    predicate = Expression.Lambda<Func<CatalogTask, bool>>(
                        Expression.AndAlso(predicate.Body,
                            Expression.Invoke(filterExpression, predicate.Parameters)), predicate.Parameters[0]);
                    break;
                }
            }
        }

        var queryable = Context.Set<CatalogTask>()
            .Where(task => task.OriginalTaskId == null)
            .OrderByDescending(task => task.DateCreated)
            .ToList()
            .Where(predicate.Compile());

        var paginatedList = queryable
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return paginatedList;
    }
}