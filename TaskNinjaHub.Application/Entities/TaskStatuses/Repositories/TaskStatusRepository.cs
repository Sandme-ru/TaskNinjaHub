using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.TaskStatuses.Repositories;

/// <summary>
/// Class TaskStatusRepository.
/// Implements the <see cref="TaskNinjaHub.Application.Entities.Bases.Repositories.BaseRepository{TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus}" />
/// Implements the <see cref="ITaskStatusRepository" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Repositories.BaseRepository{TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus}" />
/// <seealso cref="ITaskStatusRepository" />
public class TaskStatusRepository : BaseRepository<Domain.TaskStatus>, ITaskStatusRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskStatusRepository" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public TaskStatusRepository(IApplicationDbContext? context) : base((DbContext)context!)
    {

    }
}