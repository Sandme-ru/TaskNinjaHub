using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;

namespace TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;

public interface ITaskStatusRepository : IBaseRepository<CatalogTaskStatus>;