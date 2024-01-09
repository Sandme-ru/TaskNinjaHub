using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.WebClient.Services.Bases;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.WebClient.Services;

public class TaskStatusService : BaseService<TaskStatus>
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(TaskStatus).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(TaskStatus).ToLower()}";

    #endif

    public TaskStatusService(HttpClient? httpClient) : base(httpClient)
    {
    }
}