using TaskNinjaHub.WebClient.Services.Bases;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.WebClient.Services;

/// <summary>
/// Class TaskStatusService.
/// Implements the <see cref="TaskNinjaHub.WebClient.Services.Bases.BaseService{TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus}" />
/// </summary>
/// <seealso cref="TaskNinjaHub.WebClient.Services.Bases.BaseService{TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus}" />
public class TaskStatusService : BaseService<TaskStatus>
{
    /// <summary>
    /// Gets the base path.
    /// </summary>
    /// <value>The base path.</value>
    protected override string BasePath => nameof(TaskStatus).ToLower();

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskStatusService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public TaskStatusService(HttpClient? httpClient) : base(httpClient)
    {
    }
}