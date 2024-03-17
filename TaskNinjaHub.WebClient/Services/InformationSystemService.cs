using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class InformationSystemService(HttpClient httpClient) : BaseService<InformationSystem>(httpClient)
{
    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(InformationSystem).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(InformationSystem).ToLower()}";

    #endif
}