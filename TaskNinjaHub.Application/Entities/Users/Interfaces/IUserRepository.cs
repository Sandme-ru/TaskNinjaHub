using TaskNinjaHub.Application.Entities.Bases.Interfaces;

namespace TaskNinjaHub.Application.Entities.Users.Interfaces;

/// <summary>
/// Interface IUserRepository
/// Extends the <see cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Users.Domain.User}" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Users.Domain.User}" />
public interface IUserRepository : IBaseRepository<Domain.User>
{
}