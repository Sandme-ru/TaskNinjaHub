using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.Application.Entities.Users.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Users.Repositories;

/// <summary>
/// Class UserRepository.
/// Implements the <see cref="User" />
/// Implements the <see cref="IUserRepository" />
/// </summary>
/// <seealso cref="User" />
/// <seealso cref="IUserRepository" />
public class UserRepository : BaseRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public UserRepository(IApplicationDbContext? context) : base((DbContext)context!)
    {
    }
}