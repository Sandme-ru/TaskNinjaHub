﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Enum;
using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Tasks.Repositories;

public class TaskRepository(ITaskNinjaHubDbContext context, IEmailService emailService) : BaseRepository<CatalogTask>((DbContext)context), ITaskRepository
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

    public override async Task<OperationResult<CatalogTask>> AddAsync(CatalogTask entity)
    {
        try
        {
            await Context.Set<CatalogTask>().AddAsync(entity);
            await Context.SaveChangesAsync();

            entity.TaskAuthor = await Context.Set<Author>().FindAsync(entity.TaskAuthorId);
            entity.TaskExecutor = await Context.Set<Author>().FindAsync(entity.TaskExecutorId);
            entity.Priority = await Context.Set<Priority>().FindAsync(entity.PriorityId);
            entity.InformationSystem = await Context.Set<InformationSystem>().FindAsync(entity.InformationSystemId);
            entity.TaskStatus = await Context.Set<CatalogTaskStatus>().FindAsync(entity.TaskStatusId);
            entity.TaskType = await Context.Set<CatalogTaskType>().FindAsync(entity.TaskTypeId);
            try
            {
                await emailService.SendCreateEmailAsync(entity);
            }
            catch
            {

            }

            return OperationResult<CatalogTask>.SuccessResult(entity);
        }
        catch (Exception ex)
        {
            return OperationResult<CatalogTask>.FailedResult($"Failed to add entity: {ex.Message}");
        }
    }

    public async Task<OperationResult<CatalogTask>> CreateChangelogTask(CatalogTask entity, bool isUpdated)
    {
        try
        {
            await Context.Set<CatalogTask>().AddAsync(entity);
            await Context.SaveChangesAsync();

            entity.TaskAuthor = await Context.Set<Author>().FindAsync(entity.TaskAuthorId);
            entity.TaskExecutor = await Context.Set<Author>().FindAsync(entity.TaskExecutorId);
            entity.Priority = await Context.Set<Priority>().FindAsync(entity.PriorityId);
            entity.InformationSystem = await Context.Set<InformationSystem>().FindAsync(entity.InformationSystemId);
            entity.TaskStatus = await Context.Set<CatalogTaskStatus>().FindAsync(entity.TaskStatusId);
            entity.TaskType = await Context.Set<CatalogTaskType>().FindAsync(entity.TaskTypeId);

            if (!isUpdated)
                await emailService.SendCreateEmailAsync(entity);

            return OperationResult<CatalogTask>.SuccessResult(entity);
        }
        catch (Exception ex)
        {
            return OperationResult<CatalogTask>.FailedResult($"Failed to add entity: {ex.Message}");
        }
    }

    public override async Task<OperationResult<CatalogTask>> UpdateAsync(CatalogTask entity)
    {
        try
        {
            switch (entity.TaskStatusId)
            {
                case (int)EnumTaskStatus.AtWork:
                    entity.DateStart = DateTime.Now;
                    break;
                case (int)EnumTaskStatus.Done:
                    entity.DateEnd = DateTime.Now;
                    break;
            }

            Context.Set<CatalogTask>().Update(entity);
            await Context.SaveChangesAsync();

            entity.TaskAuthor = await Context.Set<Author>().FindAsync(entity.TaskAuthorId);
            entity.TaskExecutor = await Context.Set<Author>().FindAsync(entity.TaskExecutorId);
            entity.TaskType = await Context.Set<CatalogTaskType>().FindAsync(entity.TaskTypeId);

            await emailService.SendUpdateEmailAsync(entity);

            return OperationResult<CatalogTask>.SuccessResult(entity);
        }
        catch (Exception ex)
        {
            return OperationResult<CatalogTask>.FailedResult($"Failed to update entity: {ex.Message}");
        }
    }

    public override async Task<OperationResult<CatalogTask>> RemoveAsync(CatalogTask task)
    {
        try
        {
            var catalogTasks = await Context.Set<CatalogTask>().Where(t => t.OriginalTaskId == task.Id).ToListAsync();
            catalogTasks.Add(task);

            Context.Set<CatalogTask>().RemoveRange(catalogTasks);
            await Context.SaveChangesAsync();

            return OperationResult<CatalogTask>.SuccessResult();
        }
        catch (Exception ex)
        {
            return OperationResult<CatalogTask>.FailedResult($"Failed to remove entity: {ex.Message}");
        }
    }
}