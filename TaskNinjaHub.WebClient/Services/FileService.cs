using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using FileOwnershipDto = TaskNinjaHub.Application.Entities.Files.Dto.FileOwnershipDto;

namespace TaskNinjaHub.WebClient.Services;

public class FileService
{
    private string BasePath => nameof(File).ToLower();

    private readonly HttpClient _httpClient;

    public FileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<File>?> GetAllByTaskIdAsync(int taskId)
    {
        var result = await _httpClient?.GetFromJsonAsync<IEnumerable<File>>($"api/{BasePath}?taskId={taskId}")!;
        return result;
    }

    public async Task<HttpResponseMessage> ChangeOwnershipAsync(int fileId, int taskId)
    {
        var content = new FileOwnershipDto() { FileId = fileId, TaskId = taskId };
        var result = await _httpClient?.PutAsJsonAsync($"api/{BasePath}/owner-change", content)!;
        return result;
    }

    public async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        var result = await _httpClient?.DeleteAsync($"api/{BasePath}/{id}")!;
        return result;
    }
}