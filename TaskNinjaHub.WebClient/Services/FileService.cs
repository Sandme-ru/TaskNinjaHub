using TaskNinjaHub.WebClient.Services.Bases;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebClient.Services;

public class FileService(IHttpClientFactory httpClientFactory) : BaseService<File>(httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");

    #if (DEBUG)

    protected override string BasePath => $"api/{nameof(File).ToLower()}";

    #elif (RELEASE)

    protected override string BasePath => $"task-api/api/{nameof(File).ToLower()}";

    #endif

}