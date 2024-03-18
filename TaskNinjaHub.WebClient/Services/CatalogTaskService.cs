using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class CatalogTaskService(IHttpClientFactory httpClientFactory) : BaseService<CatalogTask>(httpClientFactory)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(CatalogTask).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(CatalogTask).ToLower()}";

    #endif
}