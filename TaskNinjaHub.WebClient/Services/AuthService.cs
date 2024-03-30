using Newtonsoft.Json;
using TaskNinjaHub.Application.Entities.Authors.Dto;
using TaskNinjaHub.WebClient.Data;

namespace TaskNinjaHub.WebClient.Services;

public class AuthService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AuthClient");

    public async Task<BaseResult> EditUserAsync(AuthorDto entity)
    {
        var response = await _httpClient.PostAsJsonAsync("Admin/EditUser", entity);
        var stringContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<BaseResult>(stringContent);

        return result ?? new BaseResult();
    }
}