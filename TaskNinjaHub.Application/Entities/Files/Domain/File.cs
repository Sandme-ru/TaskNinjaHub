using System.Text.Json.Serialization;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.Files.Domain;

/// <summary>
/// Class File.
/// Implements the <see cref="IHaveId" />
/// Implements the <see cref="IHaveName" />
/// Implements the <see cref="IHaveDateCreated" />
/// </summary>
/// <seealso cref="IHaveId" />
/// <seealso cref="IHaveName" />
/// <seealso cref="IHaveDateCreated" />
public class File : IHaveId, IHaveName, IHaveDateCreated
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the path to file.
    /// </summary>
    /// <value>The path to file.</value>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    /// <value>The date created.</value>
    public DateTime? DateCreated { get; set; }
    
    /// <summary>
    /// Gets or sets the task identifier.
    /// </summary>
    /// <value>The task status identifier.</value>
    public int? TaskId { get; set; }

    /// <summary>
    /// Gets or sets the task.
    /// </summary>
    /// <value>The task.</value>
    public virtual CatalogTask? Task { get; set; }
}