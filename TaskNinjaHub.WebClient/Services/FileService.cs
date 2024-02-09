using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using FileOwnershipDto = TaskNinjaHub.Application.Entities.Files.Dto.FileOwnershipDto;

namespace TaskNinjaHub.WebClient.Services;

public class FileService(HttpClient httpClient)
{
    #if (DEBUG)

    protected string BasePath => $"api/{nameof(File).ToLower()}";

    #elif (RELEASE)

    protected string BasePath => $"task-api/api/{nameof(File).ToLower()}";

    #endif

    public async Task<IEnumerable<File>?> GetAllByTaskIdAsync(int taskId)
    {
        var result = await httpClient.GetFromJsonAsync<IEnumerable<File>>($"{BasePath}?taskId={taskId}")!;
        return result;
    }

    public async Task<HttpResponseMessage> ChangeOwnershipAsync(int fileId, int taskId)
    {
        var content = new FileOwnershipDto
        {
            FileId = fileId, 
            TaskId = taskId
        };

        var result = await httpClient.PutAsJsonAsync($"{BasePath}/owner-change", content)!;
        return result;
    }

    public async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        var result = await httpClient.DeleteAsync($"{BasePath}/{id}")!;
        return result;
    }
}