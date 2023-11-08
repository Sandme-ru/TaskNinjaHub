using TaskNinjaHub.Application.Entities.Bases.Interfaces;

namespace TaskNinjaHub.Application.Entities.Files.Interfaces;

/// <summary>
/// Interface IFileRepository
/// Extends the <see cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Files.Domain.File}" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Files.Domain.File}" />
public interface IFileRepository : IBaseRepository<Domain.File>
{
    
}