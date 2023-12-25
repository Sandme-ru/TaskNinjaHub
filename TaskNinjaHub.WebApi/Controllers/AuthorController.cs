using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Authors.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

/// <summary>
/// Class AuthorController.
/// Implements the <see cref="IAuthorRepository" />
/// </summary>
/// <seealso cref="IAuthorRepository" />
public class AuthorController : BaseController<Author, IAuthorRepository>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorController"/> class.
    /// </summary>
    /// <param name="repository">The repository.</param>
    public AuthorController(IAuthorRepository repository) : base(repository)
    {
    }
}