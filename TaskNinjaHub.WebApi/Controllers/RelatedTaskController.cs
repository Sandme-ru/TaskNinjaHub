using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.RelatedTasks.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class RelatedTaskController(IRelatedTaskRepository repository) : BaseController<RelatedTask, IRelatedTaskRepository>(repository);