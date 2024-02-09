using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class CatalogTaskStatusController(ITaskStatusRepository repository) : BaseController<CatalogTaskStatus, ITaskStatusRepository>(repository);