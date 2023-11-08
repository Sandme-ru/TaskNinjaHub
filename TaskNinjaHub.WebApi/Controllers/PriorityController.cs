using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

/// <summary>
/// Class PriorityController.
/// Implements the <see cref="IPriorityRepository" />
/// </summary>
/// <seealso cref="IPriorityRepository" />
public class PriorityController : BaseController<Priority, IPriorityRepository>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PriorityController"/> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    public PriorityController(IPriorityRepository repository) : base(repository)
    {

    }
}