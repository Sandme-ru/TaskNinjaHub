namespace TaskNinjaHub.WebClient.Services.Bases;

/// <summary>
/// Interface IBaseService
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
public interface IBaseService<TEntity> where TEntity : class
{
    /// <summary>
    /// Gets all asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;IEnumerable&lt;TEntity&gt;&gt;&gt;.</returns>
    public Task<IEnumerable<TEntity>?> GetAllAsync();

    /// <summary>
    /// Get all by model as an asynchronous operation.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;IEnumerable&lt;TEntity&gt;&gt;&gt;.</returns>
    public Task<IEnumerable<TEntity>?> GetAllByFilterAsync(TEntity filterModel);

    /// <summary>
    /// Gets the identifier asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;System.Nullable&lt;TEntity&gt;&gt;.</returns>
    public Task<TEntity?> GetIdAsync(int id);

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
    public Task<HttpResponseMessage> DeleteAsync(int id);

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
    public Task<HttpResponseMessage> CreateAsync(TEntity entity);

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
    public Task<HttpResponseMessage> UpdateAsync(TEntity entity);
}