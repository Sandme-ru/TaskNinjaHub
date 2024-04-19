using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services.HttpClientServices;

public class RelatedTaskService(IHttpClientFactory httpClientFactory) : BaseService<RelatedTask>(httpClientFactory)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(RelatedTask).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(RelatedTask).ToLower()}";

    #endif
}