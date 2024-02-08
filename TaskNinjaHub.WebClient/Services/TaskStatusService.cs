using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class TaskStatusService : BaseService<CatalogTaskStatus>
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(CatalogTaskStatus).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(CatalogTaskStatus).ToLower()}";

    #endif

    public TaskStatusService(HttpClient? httpClient) : base(httpClient)
    {
    }
}