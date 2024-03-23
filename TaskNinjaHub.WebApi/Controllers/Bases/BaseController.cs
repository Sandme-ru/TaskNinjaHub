using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Filters;
using TaskNinjaHub.Application.Interfaces.Haves;
using TaskNinjaHub.Application.Utilities.OperationResults;

namespace TaskNinjaHub.WebApi.Controllers.Bases;

[ApiController]
[Route("api/[controller]")]
public class BaseController<TEntity, TRepository>(TRepository repository) : ControllerBase
    where TEntity : class, IHaveId
    where TRepository : class, IBaseRepository<TEntity>
{
    [HttpGet]
    public async Task<IEnumerable<TEntity>?> GetAll()
    {
        var entities = await repository.GetAllAsync();
        return entities ?? null;
    }
    
    [HttpGet("GetAllCount")]
    public virtual async Task<int> GetAllCount()
    {
        var count = (await repository.GetAllAsync() ?? new List<TEntity>()).ToList().Count;
        return count;
    }

    [HttpGet("GetAllByPage")]
    public async Task<IEnumerable<TEntity>?> GetAllByPage([FromQuery] FilterModel filterModel)
    {
        var entities = await repository.GetAllByPageAsync(filterModel.PageNumber, filterModel.PageSize);
        return entities ?? null;
    }

    [HttpGet("filter")]
    public async Task<IEnumerable<TEntity>?> GetAllByFilter([FromQuery] IDictionary<string, string?> query)
    {
        var entities = await repository.GetAllByFilterAsync(query);
        return entities ?? null;
    }
    
    [HttpPost("FilterByPage")]
    public async Task<IEnumerable<TEntity>?> GetAllByFilterByPage([FromBody] IDictionary<string, string?> query,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var entities = await repository.GetAllByFilterByPageAsync(query, pageNumber, pageSize);
        return entities ?? null;
    }

    [HttpGet("{id:int}")]
    public async Task<TEntity?> Get(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity ?? null;
    }

    [HttpPost("Create")]
    public async Task<OperationResult<TEntity>?> Create([FromBody] TEntity? entity)
    {
        if (entity == null)
            return OperationResult<TEntity>.FailedResult("An empty object was passed to add");

        var result = await repository.AddAsync(entity);
        return result;
    }

    [HttpPut("UpdateAsync")]
    public async Task<OperationResult<TEntity>?> Put([FromBody] TEntity? entity)
    {
        if (entity == null)
            return OperationResult<TEntity>.FailedResult("An empty object was passed to update");

        var result = await repository.UpdateAsync(entity);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TEntity>?> Delete(int id)
    {
        var entity = (repository.FindAsync(x => x.Id == id).Result ?? Array.Empty<TEntity>()).FirstOrDefault();

        if (entity != null)
        {
            var result = await repository.RemoveAsync(entity);

            if (result.Success)
                return Ok(entity);
            else
                return BadRequest(result.ErrorMessage);
        }

        return BadRequest("The object to delete was not found");
    }
}