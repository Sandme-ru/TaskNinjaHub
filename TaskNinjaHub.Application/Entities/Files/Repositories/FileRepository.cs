using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Files.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Files.Repositories;

public class FileRepository : BaseRepository<Domain.File>, IFileRepository
{
    public FileRepository(ITaskNinjaHubDbContext? context) : base((DbContext)context!)
    {
        
    }
}