using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.Application.Entities.Tasks.Interfaces;

public interface ITaskRepository : IBaseRepository<CatalogTask>
{
    Task<OperationResult<CatalogTask>> CreateSameTask(CatalogTask entity, bool isUpdated);
}
