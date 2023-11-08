using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Entities.Tasks.Domain;

namespace TaskNinjaHub.Application.Entities.Tasks.Interfaces;

/// <summary>
/// Interface ITaskRepository
/// Extends the <see cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Tasks.Domain.CatalogTask}" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Tasks.Domain.CatalogTask}" />
public interface ITaskRepository : IBaseRepository<CatalogTask>
{
    
}