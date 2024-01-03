using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Tasks.Repositories;

/// <summary>
/// Class TaskRepository.
/// Implements the <see cref="CatalogTask" />
/// Implements the <see cref="ITaskRepository" />
/// </summary>
/// <seealso cref="CatalogTask" />
/// <seealso cref="ITaskRepository" />
public class TaskRepository : BaseRepository<CatalogTask>, ITaskRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public TaskRepository(ITaskNinjaHubDbContext? context) : base((DbContext)context!)
    {
        
    }
}