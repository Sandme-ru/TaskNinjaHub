namespace TaskNinjaHub.WebClient.Services.Bases;

public interface IBaseService<TEntity> where TEntity : class
{
    public Task<IEnumerable<TEntity>?> GetAllAsync();

    public Task<IEnumerable<TEntity>?> GetAllByFilterAsync(TEntity filterModel);

    public Task<TEntity?> GetIdAsync(int id);

    public Task<HttpResponseMessage> DeleteAsync(int id);

    public Task<HttpResponseMessage> CreateAsync(TEntity entity);

    public Task<HttpResponseMessage> UpdateAsync(TEntity entity);
}