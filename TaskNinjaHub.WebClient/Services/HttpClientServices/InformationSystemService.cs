using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services.HttpClientServices;

public class InformationSystemService(IHttpClientFactory httpClientFactory) : BaseService<InformationSystem>(httpClientFactory)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(InformationSystem).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(InformationSystem).ToLower()}";

    #endif
}