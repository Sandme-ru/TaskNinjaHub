using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class TaskStatusService(HttpClient httpClient) : BaseService<CatalogTaskStatus>(httpClient)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(CatalogTaskStatus).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(CatalogTaskStatus).ToLower()}";

    #endif
}