using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using TaskNinjaHub.Application.Entities.TaskTypes.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class CatalogTaskTypeController(ITaskTypeRepository repository) : BaseController<CatalogTaskType, ITaskTypeRepository>(repository);