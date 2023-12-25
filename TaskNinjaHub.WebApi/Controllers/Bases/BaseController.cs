using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.WebApi.Controllers.Bases;

/// <summary>
/// Class BaseController.
/// Implements the <see cref="ControllerBase" />
/// </summary>
/// <typeparam name="TEntity">The type of the t entity.</typeparam>
/// <typeparam name="TRepository">The type of the t repository.</typeparam>
/// <seealso cref="ControllerBase" />
[ApiController]
[Route("api/[controller]")]
public class BaseController<TEntity, TRepository> : ControllerBase
    where TEntity : class, IHaveId
    where TRepository : class, IBaseRepository<TEntity>
{
    /// <summary>
    /// The repository
    /// </summary>
    private readonly TRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseController{TEntity, TRepository}"/> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    public BaseController(TRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns>System.Nullable&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
    [HttpGet]
    public async Task<IEnumerable<TEntity>?> GetAll()
    {
        var entities = await _repository.GetAllAsync();
        return entities ?? null;
    }

    /// <summary>
    /// Gets all by filterModel.
    /// </summary>
    /// <returns>System.Nullable&lt;IEnumerable&lt;TEntity&gt;&gt;.</returns>
    [HttpGet("filter")]
    public async Task<IEnumerable<TEntity>?> GetAllByFilter([FromQuery] IDictionary<string, string?> query)
    {
        var entities = await _repository.GetAllByFilterAsync(query);
        return entities ?? null;
    }

    /// <summary>
    /// Gets the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>System.Nullable&lt;TEntity&gt;.</returns>
    [HttpGet("{id:int}")]
    public async Task<TEntity?> Get(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity ?? null;
    }

    /// <summary>
    /// Creates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>System.Nullable&lt;TEntity&gt;.</returns>
    [HttpPost("Create")]
    public async Task<TEntity?> Create([FromBody] TEntity? entity)
    {
        if (entity == null)
            return null;

        await _repository.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Puts the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>System.Nullable&lt;TEntity&gt;.</returns>
    [HttpPut("UpdateAsync")]
    public async Task<TEntity?> Put([FromBody] TEntity? entity)
    {
        if (entity == null)
            return null;

        await _repository.UpdateAsync(entity);
        return entity;
    }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>TEntity.</returns>
    [HttpDelete("{id}")]
    public async Task<TEntity?> Delete(int id)
    {
        var entity = (_repository.FindAsync(x => x.Id == id).Result ?? Array.Empty<TEntity>()).FirstOrDefault();

        if (entity != null)
        {
            await _repository.RemoveAsync(entity);
            return entity;
        }

        return null;
    }
}