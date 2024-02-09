using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class PriorityService(HttpClient? httpClient) : BaseService<Priority>(httpClient)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(Priority).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(Priority).ToLower()}";

    #endif
}