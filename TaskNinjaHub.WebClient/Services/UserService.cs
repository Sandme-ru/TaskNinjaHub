using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

/// <summary>
/// Class UserService.
/// Implements the <see cref="User" />
/// </summary>
/// <seealso cref="User" />
public class UserService : BaseService<User>
{
    private readonly HttpClient? _httpClient;
    /// <summary>
    /// Gets the base path.
    /// </summary>
    /// <value>The base path.</value>
    protected override string BasePath => nameof(User).ToLower();

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public UserService(HttpClient? httpClient) : base(httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Uploads image as asynchronous operation.
    /// </summary>
    /// <param name="content">The image.</param>
    /// <returns>A Task&lt;HtttpResponseMessage; representing the asynchronous operation.</returns>
    public async Task<HttpResponseMessage> UploadAvatarAsync(MultipartFormDataContent content)
    {
        var result = await _httpClient?.PostAsync($"api/{BasePath}/UploadAvatar", content)!;
        return result;
    }

    /// <summary>
    /// Retrieves image as asynchronous operation.
    /// </summary>
    /// <param name="avatarPath">The path to the image.</param>
    /// <returns>A Task&lt;HtttpResponseMessage; representing the asynchronous operation.</returns>
    public async Task<HttpResponseMessage> GetAvatarAsync(string avatarPath)
    {
        var result = await _httpClient!.GetAsync($"api/{BasePath}/GetAvatar/{avatarPath}");
        return result;

    }
}