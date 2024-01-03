using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Files.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Files.Repositories;

/// <summary>
/// Class FileRepository.
/// Implements the <see cref="TaskNinjaHub.Application.Entities.Bases.Repositories.BaseRepository{TaskNinjaHub.Application.Entities.Files.Domain.File}" />
/// Implements the <see cref="IFileRepository" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Repositories.BaseRepository{TaskNinjaHub.Application.Entities.Files.Domain.File}" />
/// <seealso cref="IFileRepository" />
public class FileRepository : BaseRepository<Domain.File>, IFileRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public FileRepository(ITaskNinjaHubDbContext? context) : base((DbContext)context!)
    {
        
    }
}