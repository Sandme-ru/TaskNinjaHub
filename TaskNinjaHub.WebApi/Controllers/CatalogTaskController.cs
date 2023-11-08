using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

/// <summary>
/// Class CatalogTaskController.
/// Implements the <see cref="ITaskRepository" />
/// </summary>
/// <seealso cref="ITaskRepository" />
public class CatalogTaskController : BaseController<CatalogTask, ITaskRepository>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogTaskController"/> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    public CatalogTaskController(ITaskRepository repository) : base(repository)
    {
    }
}