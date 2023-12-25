using System.Text.Json.Serialization;
using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Entities.Roles.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.Authors.Domain;

/// <summary>
/// Class Author.
/// Implements the <see cref="BaseUserCU" />
/// Implements the <see cref="IHaveId" />
/// Implements the <see cref="IHaveName" />
/// </summary>
/// <seealso cref="BaseUserCU" />
/// <seealso cref="IHaveId" />
/// <seealso cref="IHaveName" />
public class Author: BaseUserCU, IHaveId, IHaveName
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
    /// Gets or sets the role identifier.
    /// </summary>
    /// <value>The role identifier.</value>
    public int? RoleId { get; set; }

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    /// <value>The role.</value>
    public virtual Role? Role { get; set; }

    /// <summary>
    /// Gets or sets the executable tasks.
    /// </summary>
    /// <value>The executable tasks.</value>
    [JsonIgnore]
    public virtual List<CatalogTask>? ExecutableTasks { get; set; }

    /// <summary>
    /// Gets or sets the assigned tasks.
    /// </summary>
    /// <value>The assigned tasks.</value>
    [JsonIgnore]
    public virtual List<CatalogTask>? AssignedTasks { get; set; }
}