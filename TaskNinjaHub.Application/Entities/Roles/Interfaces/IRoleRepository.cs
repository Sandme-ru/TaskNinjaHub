using TaskNinjaHub.Application.Entities.Bases.Interfaces;
using TaskNinjaHub.Application.Entities.Roles.Domain;

namespace TaskNinjaHub.Application.Entities.Roles.Interfaces;

/// <summary>
/// Interface IRoleRepository
/// Extends the <see cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Roles.Domain.Role}" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Roles.Domain.Role}" />
public interface IRoleRepository : IBaseRepository<Role>
{

}