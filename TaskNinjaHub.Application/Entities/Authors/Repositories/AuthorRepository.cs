using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Authors.Interfaces;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Authors.Repositories;

/// <summary>
/// Class AuthorRepository.
/// Implements the <see cref="Author" />
/// Implements the <see cref="IAuthorRepository" />
/// </summary>
/// <seealso cref="Author" />
/// <seealso cref="IAuthorRepository" />
public class AuthorRepository: BaseRepository<Author>, IAuthorRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public AuthorRepository(IApplicationDbContext? context) : base((DbContext)context!)
    {

    }
}