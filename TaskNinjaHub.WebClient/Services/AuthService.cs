using TaskNinjaHub.Application.Entities.Authors.Dto;

namespace TaskNinjaHub.WebClient.Services;

public class AuthService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthClient");

    public async Task<HttpResponseMessage> EditUserAsync(AuthorDto entity)
    {
        var result = await _httpClient.PostAsJsonAsync($"Admin/EditUser", entity)!;
        return result;
    }
}