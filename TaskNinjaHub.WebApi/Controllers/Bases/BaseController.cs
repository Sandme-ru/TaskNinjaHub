using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.WebApi.Controllers.Bases;

[ApiController]
[Route("api/[controller]")]
public class BaseController<TEntity, TRepository> : ControllerBase
    where TEntity : class, IHaveId
    where TRepository : class, IBaseRepository<TEntity>
{
    private readonly TRepository _repository;

    public BaseController(TRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<TEntity>?> GetAll()
    {
        var entities = await _repository.GetAllAsync();
        return entities ?? null;
    }
    
    [HttpGet("GetAllCount")]
    public async Task<int> GetAllCount()
    {
        var allCount = (await _repository.GetAllAsync() ?? Array.Empty<TEntity>())
            .ToList().Count;
        return allCount;
    }

    [HttpGet("GetAllByPage")]
    public async Task<IEnumerable<TEntity>?> GetAllByPage([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var entities = await _repository.GetAllByPageAsync(pageNumber, pageSize);
        return entities ?? null;
    }

    [HttpGet("filter")]
    public async Task<IEnumerable<TEntity>?> GetAllByFilter([FromQuery] IDictionary<string, string?> query)
    {
        var entities = await _repository.GetAllByFilterAsync(query);
        return entities ?? null;
    }

    [HttpGet("{id:int}")]
    public async Task<TEntity?> Get(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity ?? null;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<TEntity>?> Create([FromBody] TEntity? entity)
    {
        if (entity == null)
            return BadRequest("An empty object was passed to add");

        var result = await _repository.AddAsync(entity);

        if (result.Success)
            return Ok(entity);
        else
            return BadRequest(result.ErrorMessage);
    }

    [HttpPut("UpdateAsync")]
    public async Task<ActionResult<TEntity>?> Put([FromBody] TEntity? entity)
    {
        if (entity == null)
            return BadRequest("An empty object was passed to update");

        var result = await _repository.UpdateAsync(entity);

        if (result.Success)
            return Ok(entity);
        else
            return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TEntity>?> Delete(int id)
    {
        var entity = (_repository.FindAsync(x => x.Id == id).Result ?? Array.Empty<TEntity>()).FirstOrDefault();

        if (entity != null)
        {
            var result = await _repository.RemoveAsync(entity);

            if (result.Success)
                return Ok(entity);
            else
                return BadRequest(result.ErrorMessage);
        }

        return BadRequest("The object to delete was not found");
    }
}