using Microsoft.AspNetCore.WebUtilities;
using TaskNinjaHub.WebClient.Utilities;

namespace TaskNinjaHub.WebClient.Services.Bases;

/// <summary>
/// Class BaseService.
/// Implements the <see cref="IBaseService{TEntity}" />
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <seealso cref="IBaseService{TEntity}" />
public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private readonly HttpClient? _httpClient;

    /// <summary>
    /// Gets the base path.
    /// </summary>
    /// <value>The base path.</value>
    protected virtual string BasePath => nameof(TEntity).ToLower();

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseService{TEntity}"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    protected BaseService(HttpClient? httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Get all as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
    public virtual async Task<IEnumerable<TEntity>?> GetAllAsync()
    {
        var result = await _httpClient?.GetFromJsonAsync<IEnumerable<TEntity>>($"api/{BasePath}")!;
        return result;
    }
    
    /// <summary>
    /// Get all by model as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
    public virtual async Task<IEnumerable<TEntity>?> GetAllByFilterAsync(TEntity filterModel)
    {
        var requestUri = QueryHelpers.AddQueryString($"api/{BasePath}/filter", filterModel.ToPropertyDictionary());
        var result = await _httpClient?.GetFromJsonAsync<IEnumerable<TEntity>>(requestUri)!;
        return result;
    }

    /// <summary>
    /// Get identifier as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>A Task&lt;TEntity&gt; representing the asynchronous operation.</returns>
    public virtual async Task<TEntity?> GetIdAsync(int id)
    {
        var result = await _httpClient!.GetFromJsonAsync<TEntity>($"api/{BasePath}/{id}");
        return result;
    }

    /// <summary>
    /// Delete as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>A Task&lt;HttpResponseMessage&gt; representing the asynchronous operation.</returns>
    public virtual async Task<HttpResponseMessage> DeleteAsync(int id)
    {
        var result = await _httpClient?.DeleteAsync($"api/{BasePath}/{id}")!;
        return result;
    }

    /// <summary>
    /// Create as an asynchronous operation.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A Task&lt;HttpResponseMessage&gt; representing the asynchronous operation.</returns>
    public virtual async Task<HttpResponseMessage> CreateAsync(TEntity entity)
    {
        var result = await _httpClient?.PostAsJsonAsync($"api/{BasePath}/Create", entity)!;
        return result;
    }

    /// <summary>
    /// Update as an asynchronous operation.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A Task&lt;HttpResponseMessage&gt; representing the asynchronous operation.</returns>
    public virtual async Task<HttpResponseMessage> UpdateAsync(TEntity entity)
    {
        var result = await _httpClient?.PutAsJsonAsync($"api/{BasePath}/Update", entity)!;
        return result;
    }
}