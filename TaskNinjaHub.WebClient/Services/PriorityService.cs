using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class PriorityService(IHttpClientFactory httpClientFactory) : BaseService<Priority>(httpClientFactory)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(Priority).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(Priority).ToLower()}";

    #endif
}