using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Priorities.Repositories;

/// <summary>
/// Class PriorityRepository.
/// Implements the <see cref="Priority" />
/// Implements the <see cref="IPriorityRepository" />
/// </summary>
/// <seealso cref="Priority" />
/// <seealso cref="IPriorityRepository" />
public class PriorityRepository : BaseRepository<Domain.Priority>, IPriorityRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PriorityRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public PriorityRepository(IApplicationDbContext? context) : base((DbContext)context!)
    {

    }
}