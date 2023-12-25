using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using FileOwnershipDto = TaskNinjaHub.Application.Entities.Files.Dto.FileOwnershipDto;

namespace TaskNinjaHub.WebClient.Services;

/// <summary>
/// Class FileService.
/// Implements the <see cref="File" />
/// </summary>
/// <seealso cref="File" />
public class FileService
{
    /// <summary>
    /// Gets the base path.
    /// </summary>
    /// <value>The base path.</value>
    private string BasePath => nameof(File).ToLower();

    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogTaskService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public FileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get all by task ID as an asynchronous operation.
    /// </summary>
    /// <param name="taskId">The task identifier.</param>
    /// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
    public async Task<IEnumerable<File>?> GetAllByTaskIdAsync(int taskId)
    {
        var result = await _httpClient?.GetFromJsonAsync<IEnumerable<File>>($"api/{BasePath}?taskId={taskId}")!;
        return result;
    }

    /// <summary>
    /// Method that allows you to determine the task to which the file belongs.
    /// </summary>
    /// <param name="fileId">The file id.</param>
    /// <param name="taskId">The task id.</param>
    /// <returns>A Task&lt;HttpResponseMessage&gt; representing the asynchronous operation.</returns>
    public async Task<HttpResponseMessage> ChangeOwnershipAsync(int fileId, int taskId)
    {
        var content = new FileOwnershipDto() { FileId = fileId, TaskId = taskId };
        var result = await _httpClient?.PutAsJsonAsync($"api/{BasePath}/owner-change", content)!;
        return result;
    }

    /// <summary>
    /// Delete as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>A Task&lt;HttpResponseMessage&gt; representing the asynchronous operation.</returns>
    public async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        var result = await _httpClient?.DeleteAsync($"api/{BasePath}/{id}")!;
        return result;
    }
}