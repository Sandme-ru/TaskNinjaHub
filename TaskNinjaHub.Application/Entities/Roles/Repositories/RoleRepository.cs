using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Roles.Domain;
using TaskNinjaHub.Application.Entities.Roles.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Roles.Repositories;

/// <summary>
/// Class RoleRepository.
/// Implements the <see cref="Role" />
/// Implements the <see cref="IRoleRepository" />
/// </summary>
/// <seealso cref="Role" />
/// <seealso cref="IRoleRepository" />
public class RoleRepository : BaseRepository<Domain.Role>, IRoleRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public RoleRepository(IApplicationDbContext? context) : base((DbContext)context!)
    {

    }
}