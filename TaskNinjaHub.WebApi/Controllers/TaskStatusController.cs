using TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class TaskStatusController : BaseController<Application.Entities.TaskStatuses.Domain.CatalogTaskStatus, ITaskStatusRepository>
{
    public TaskStatusController(ITaskStatusRepository repository) : base(repository)
    {

    }
}