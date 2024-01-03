using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.InformationSystems.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.InformationSystems.Repositories;

/// <summary>
/// Class InformationSystemRepository.
/// Implements the <see cref="InformationSystem" />
/// Implements the <see cref="IInformationSystemRepository" />
/// </summary>
/// <seealso cref="InformationSystem" />
/// <seealso cref="IInformationSystemRepository" />
public class InformationSystemRepository: BaseRepository<Domain.InformationSystem>, IInformationSystemRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InformationSystemRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public InformationSystemRepository(ITaskNinjaHubDbContext? context) : base((DbContext)context!)
    {
    }
}