using TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

/// <summary>
/// Class TaskStatusController.
/// Implements the <see cref="ITaskStatusRepository" />
/// </summary>
/// <seealso cref="ITaskStatusRepository" />
public class TaskStatusController : BaseController<Application.Entities.TaskStatuses.Domain.TaskStatus, ITaskStatusRepository>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskStatusController"/> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    public TaskStatusController(ITaskStatusRepository repository) : base(repository)
    {

    }
}