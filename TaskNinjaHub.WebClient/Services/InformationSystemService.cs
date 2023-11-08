using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

/// <summary>
/// Class InformationSystemService.
/// Implements the <see cref="InformationSystem" />
/// </summary>
/// <seealso cref="InformationSystem" />
public class InformationSystemService : BaseService<InformationSystem>
{
    /// <summary>
    /// Gets the base path.
    /// </summary>
    /// <value>The base path.</value>
    protected override string BasePath => nameof(InformationSystem).ToLower();

    /// <summary>
    /// Initializes a new instance of the <see cref="InformationSystemService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public InformationSystemService(HttpClient? httpClient) : base(httpClient)
    {
    }
}