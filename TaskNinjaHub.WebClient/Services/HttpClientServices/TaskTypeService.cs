using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services.HttpClientServices;

public class TaskTypeService(IHttpClientFactory httpClientFactory) : BaseService<CatalogTaskType>(httpClientFactory)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(CatalogTaskType).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(CatalogTaskType).ToLower()}";

    #endif
}