using TaskNinjaHub.Application.Entities.Roles.Domain;
using TaskNinjaHub.Application.Entities.Roles.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

/// <summary>
/// Class RoleController.
/// Implements the <see cref="IRoleRepository" />
/// </summary>
/// <seealso cref="IRoleRepository" />
public class RoleController : BaseController<Role, IRoleRepository>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleController"/> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    public RoleController(IRoleRepository repository) : base(repository)
    {

    }
}