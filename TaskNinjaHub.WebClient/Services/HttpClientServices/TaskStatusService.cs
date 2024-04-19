using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services.HttpClientServices;

public class TaskStatusService(IHttpClientFactory httpClientFactory) : BaseService<CatalogTaskStatus>(httpClientFactory)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(CatalogTaskStatus).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(CatalogTaskStatus).ToLower()}";

    #endif
}