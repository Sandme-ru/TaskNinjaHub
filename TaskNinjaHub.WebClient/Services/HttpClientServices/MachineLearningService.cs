using Newtonsoft.Json;
using TaskNinjaHub.Application.Entities.Tasks.Dto;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.WebClient.Services.HttpClientServices;

public class MachineLearningService(IHttpClientFactory httpClientFactory, CatalogTaskService taskService)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("MachineLearningApi");

    public async Task<OperationResult<string>?> TrainTasksModel()
    {
        var tasks = await taskService.GetAllAsync();
        var response = await _httpClient.PostAsJsonAsync("Forecast/Train", tasks);
        var stringContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult<string>>(stringContent);

        return result;
    }

    public async Task<OperationResult<double>?> PredictProbability(TaskInputDto taskInputDto)
    {
        var response = await _httpClient.PostAsJsonAsync("Forecast/PredictProbability", taskInputDto);
        var stringContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult<double>>(stringContent);

        return result;
    }
}