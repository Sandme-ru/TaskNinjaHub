using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using TaskNinjaHub.Application.Filters;
using TaskNinjaHub.WebClient.Utilities;

namespace TaskNinjaHub.WebClient.Services.Bases;

public abstract class BaseService<TEntity>(IHttpClientFactory httpClientFactory) : IBaseService<TEntity> where TEntity : class
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");

    #if (DEBUG)

    protected virtual string BasePath => $"api/{nameof(TEntity).ToLower()}";

    #elif (RELEASE)
    
    protected virtual string BasePath => $"task-api/api/{nameof(TEntity).ToLower()}";

    #endif

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<TEntity>>($"{BasePath}")!;
        return result;
    }

    public virtual async Task<int> GetAllCountAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<int>($"{BasePath}/GetAllCount")!;
        return result;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllByPageAsync(FilterModel filterModel)
    {
        if (filterModel.PageNumber < 1 || filterModel.PageSize < 1)
            return null;

        var result = await _httpClient.GetFromJsonAsync<IEnumerable<TEntity>>($"{BasePath}/GetAllByPage?pageNumber={filterModel.PageNumber}&pageSize={filterModel.PageSize}")!;
        return result;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllByFilterAsync(TEntity filterModel)
    {
        var requestUri = QueryHelpers.AddQueryString($"{BasePath}/filter", filterModel.ToPropertyDictionary());
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<TEntity>>(requestUri)!;
        return result;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllByFilterByPageAsync(TEntity filterModel, int pageNumber = 1, int pageSize = 10)
    {
        var requestUri = $"{BasePath}/FilterByPage?pageNumber={pageNumber}&pageSize={pageSize}";
        var response = await _httpClient.PostAsJsonAsync(requestUri, filterModel.ToPropertyDictionary())!;
        var result = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(await response.Content.ReadAsStringAsync());

        return result;
    }

    public virtual async Task<TEntity> GetIdAsync(int id)
    {
        var result = await _httpClient.GetFromJsonAsync<TEntity>($"{BasePath}/{id}");
        return result;
    }

    public virtual async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        var result = await _httpClient.DeleteAsync($"{BasePath}/{id}")!;
        return result;
    }

    public virtual async Task<HttpResponseMessage> CreateAsync(TEntity entity)
    {
        var result = await _httpClient.PostAsJsonAsync($"{BasePath}/Create", entity)!;
        return result;
    }

    public virtual async Task<HttpResponseMessage> UpdateAsync(TEntity entity)
    {
        var result = await _httpClient.PutAsJsonAsync($"{BasePath}/UpdateAsync", entity)!;
        return result;
    }
}