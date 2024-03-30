using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Authors.Interfaces;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Entities.TaskStatuses.Enum;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Authors.Repositories;

public class AuthorRepository(ITaskNinjaHubDbContext context, ITaskRepository taskRepository) : BaseRepository<Author>((DbContext)context), IAuthorRepository
{
    public new async Task<IEnumerable<Author>?> GetAllAsync()
    {
        var authors = await Context.Set<Author>()
            .OrderByDescending(entity => entity.Id)
            .ToListAsync();

        foreach (var author in authors)
        {
            var tasks = await taskRepository.GetAllByFilterAsync(new Dictionary<string, string?> { { "TaskExecutorId", author.Id.ToString() } });
            if (tasks != null)
            {
                tasks = tasks.Where(task => task.TaskStatusId != (int)EnumTaskStatus.Done);
                author.CountPerformedTasks = tasks.Count();
            }
        }

        return authors;
    }

    public new async Task<OperationResult<Author>> AddAsync(Author author)
    {
        try
        {
            var updatedAuthor = await context.Authors.FirstOrDefaultAsync(a => a.Name == author.Name);

            if (updatedAuthor != null)
            {
                updatedAuthor.Name = author.Name;
                updatedAuthor.RoleName = author.RoleName;
                updatedAuthor.ShortName = author.ShortName;

                context.Authors.Update(updatedAuthor);
                await ((DbContext)context).SaveChangesAsync();

                return OperationResult<Author>.SuccessResult(updatedAuthor);
            }

            context.Authors.Update(author);
            await ((DbContext)context).SaveChangesAsync();

            return OperationResult<Author>.SuccessResult(author);
        }
        catch (Exception ex)
        {
            return OperationResult<Author>.FailedResult($"Failed to add author: {ex.Message}");
        }
    }
}