using Microsoft.AspNetCore.WebUtilities;
using TaskNinjaHub.WebClient.Utilities;

namespace TaskNinjaHub.WebClient.Services.Bases;

public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    private readonly HttpClient? _httpClient;

    protected virtual string BasePath => nameof(TEntity).ToLower();

    protected BaseService(HttpClient? httpClient)
    {
        _httpClient = httpClient;
    }

    public virtual async Task<IEnumerable<TEntity>?> GetAllAsync()
    {
        var result = await _httpClient?.GetFromJsonAsync<IEnumerable<TEntity>>($"api/{BasePath}")!;
        return result;
    }

    public virtual async Task<int> GetAllCountAsync()
    {
        var result = await _httpClient?.GetFromJsonAsync<int>($"api/{BasePath}/GetAllCount")!;
        return result;
    }

    public virtual async Task<IEnumerable<TEntity>?> GetAllByPageAsync(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
            return null;

        var result = await _httpClient?.GetFromJsonAsync<IEnumerable<TEntity>>($"api/{BasePath}/GetAllByPage?pageNumber={pageNumber}&pageSize={pageSize}")!;
        return result;
    }
    
    public virtual async Task<IEnumerable<TEntity>?> GetAllByFilterAsync(TEntity filterModel)
    {
        var requestUri = QueryHelpers.AddQueryString($"api/{BasePath}/filter", filterModel.ToPropertyDictionary());
        var result = await _httpClient?.GetFromJsonAsync<IEnumerable<TEntity>>(requestUri)!;
        return result;
    }

    public virtual async Task<TEntity?> GetIdAsync(int id)
    {
        var result = await _httpClient!.GetFromJsonAsync<TEntity>($"api/{BasePath}/{id}");
        return result;
    }

    public virtual async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        var result = await _httpClient?.DeleteAsync($"api/{BasePath}/{id}")!;
        return result;
    }

    public virtual async Task<HttpResponseMessage> CreateAsync(TEntity entity)
    {
        var result = await _httpClient?.PostAsJsonAsync($"api/{BasePath}/Create", entity)!;
        return result;
    }

    public virtual async Task<HttpResponseMessage> UpdateAsync(TEntity entity)
    {
        var result = await _httpClient?.PutAsJsonAsync($"api/{BasePath}/Update", entity)!;
        return result;
    }
}