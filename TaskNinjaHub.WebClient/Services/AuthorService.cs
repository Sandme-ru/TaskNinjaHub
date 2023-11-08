using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

/// <summary>
/// Class AuthorService.
/// Implements the <see cref="Author" />
/// </summary>
/// <seealso cref="Author" />
public class AuthorService : BaseService<Author>
{
    /// <summary>
    /// Gets the base path.
    /// </summary>
    /// <value>The base path.</value>
    protected override string BasePath => nameof(Author).ToLower();

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public AuthorService(HttpClient? httpClient) : base(httpClient)
    {
    }
}