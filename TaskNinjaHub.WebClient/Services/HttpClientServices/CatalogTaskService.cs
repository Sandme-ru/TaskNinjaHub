using Newtonsoft.Json;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Utilities.OperationResults;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services.HttpClientServices;

public class CatalogTaskService(IHttpClientFactory httpClientFactory) : BaseService<CatalogTask>(httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");

    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(CatalogTask).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(CatalogTask).ToLower()}";

    #endif

    public async Task<OperationResult<CatalogTask>> CreateChangelogTaskAsync(CatalogTask entity, bool isUpdated)
    {
        var data = new { Task = entity, IsUpdated = isUpdated };
        var response = await _httpClient.PostAsJsonAsync($"{BasePath}/CreateChangelogTask", data);
        var result = JsonConvert.DeserializeObject<OperationResult<CatalogTask>>(await response.Content.ReadAsStringAsync());

        return result!;
    }
}