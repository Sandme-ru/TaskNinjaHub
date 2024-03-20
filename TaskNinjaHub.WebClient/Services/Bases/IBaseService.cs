using TaskNinjaHub.Application.Filters;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.WebClient.Services.Bases;

public interface IBaseService<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<int> GetAllCountAsync();

    Task<IEnumerable<TEntity>> GetAllByPageAsync(FilterModel filterModel);

    Task<IEnumerable<TEntity>> GetAllByFilterAsync(TEntity filterModel);

    Task<TEntity> GetIdAsync(int id);

    Task<HttpResponseMessage> DeleteAsync(int id);

    Task<OperationResult<TEntity>> CreateAsync(TEntity entity);

    Task<OperationResult<TEntity>> UpdateAsync(TEntity entity);
}