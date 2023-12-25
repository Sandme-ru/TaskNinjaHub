using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

/// <summary>
/// Class CatalogTaskService.
/// Implements the <see cref="CatalogTask" />
/// </summary>
/// <seealso cref="CatalogTask" />
public class CatalogTaskService : BaseService<CatalogTask>
{
    /// <summary>
    /// Gets the base path.
    /// </summary>
    /// <value>The base path.</value>
    protected override string BasePath => nameof(CatalogTask).ToLower();

    /// <summary>
    /// Initializes a new instance of the <see cref="CatalogTaskService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public CatalogTaskService(HttpClient? httpClient) : base(httpClient)
    {
    }
}