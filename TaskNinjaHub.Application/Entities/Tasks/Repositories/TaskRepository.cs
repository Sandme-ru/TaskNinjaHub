using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Tasks.Repositories;

public class TaskRepository : BaseRepository<CatalogTask>, ITaskRepository
{
    private readonly string _smtpServer = "smtp.mail.ru";

    private readonly int _smtpPort = 587;

    private readonly string _smtpUsername = "zhurnal.kuratora@mail.ru";

    private readonly string _smtpPassword = "wuxREz6iDKYuegwCQuEm";

    private SmtpClient SmtpClient { get; set; } = null!;

    public TaskRepository(ITaskNinjaHubDbContext context) : base((DbContext)context)
    {
        SmtpClient = new(_smtpServer, _smtpPort)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
            EnableSsl = true
        };

    }

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
            
            string htmlBody = @"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Task</title>
        <style>
            .task-container {
                width: 90%;
                max-width: 600px;
                margin: 0 auto;
                border-radius: 15px;
                background-color: #fff;
                box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.5);
                padding: 20px;
            }
            .task-info {
                text-align: center;
            }
            .task-info img {
                display: block;
                margin: 0 auto 10px;
                max-width: 40%;
            }
            .task-info h2 {
                margin-bottom: 10px;
            }
            .task-info p {
                margin-bottom: 10px;
            }
            .task-info button {
                display: block;
                width: 100%;
                padding: 10px;
                border: none;
                border-radius: 5px;
                background-color: #007bff;
                color: #fff;
                cursor: pointer;
            }
        </style>
    </head>
    <body>
        <div class=""task-container"">
            <div class=""task-info"">
                <img src=""https://sandme.ru/file-storage/api/File/GetFile?bucketName=task-files&fileName=HubLogoPNG.png"" alt=""Logo"">
                <h2>" + entity.Name + @"</h2>
                <p>Task executor: " + entity.TaskAuthor?.Name + @"</p>
                <p>Priority: " + entity.Priority + @"</p>
                <p>Information system: " + entity.InformationSystem + @"</p>
                <p>Task status: " + entity.TaskStatus + @"</p>
                <a href=""https://sandme.ru/task-read/" + entity.Id + @"""><button>View task</button></a>
            </div>
        </div>
    </body>
    </html>";

            var message = new MailMessage("zhurnal.kuratora@mail.ru", /*$"{entity.TaskExecutor?.Name}"*/ "shvyrkalovm@mail.ru", "You have been assigned a task",
                htmlBody);
            message.IsBodyHtml = true;
            SmtpClient.Send(message);

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
            Context.Set<CatalogTask>().Update(entity);
            await Context.SaveChangesAsync();

            var message = new MailMessage("zhurnal.kuratora@mail.ru", $"{entity.TaskExecutor?.Name}", "Задача обновлена",
                $"Задача с ID {entity.Id} была обновлена.");
            SmtpClient.Send(message);

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
            var catalogTasks = Context.Set<CatalogTask>().Where(t => t.OriginalTaskId == task.Id);
            catalogTasks.Append(task);

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