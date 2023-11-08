using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

/// <summary>
/// Class InformationSystemController.
/// Implements the <see cref="IInformationSystemRepository" />
/// </summary>
/// <seealso cref="IInformationSystemRepository" />
public class InformationSystemController : BaseController<InformationSystem, IInformationSystemRepository>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InformationSystemController"/> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    public InformationSystemController(IInformationSystemRepository repository) : base(repository)
    {

    }
}