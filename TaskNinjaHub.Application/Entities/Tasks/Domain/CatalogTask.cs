using System.Text.Json.Serialization;
using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.Application.Entities.Tasks.Domain;

/// <summary>
/// Class CatalogTask.
/// Implements the <see cref="BaseUserCU" />
/// Implements the <see cref="IHaveId" />
/// Implements the <see cref="IHaveName" />
/// </summary>
/// <seealso cref="BaseUserCU" />
/// <seealso cref="IHaveId" />
/// <seealso cref="IHaveName" />
public class CatalogTask : BaseUserCU, IHaveId, IHaveName
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
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the task author identifier.
    /// </summary>
    /// <value>The task author identifier.</value>
    public int? TaskAuthorId { get; set; }

    /// <summary>
    /// Gets or sets the task author.
    /// </summary>
    /// <value>The task author.</value>
    public virtual Author? TaskAuthor { get; set; }

    /// <summary>
    /// Gets or sets the task executor identifier.
    /// </summary>
    /// <value>The task executor identifier.</value>
    public int? TaskExecutorId { get; set; }

    /// <summary>
    /// Gets or sets the task executor.
    /// </summary>
    /// <value>The task executor.</value>
    public virtual Author? TaskExecutor { get; set; }

    /// <summary>
    /// Gets or sets the information system identifier.
    /// </summary>
    /// <value>The information system identifier.</value>
    public int? InformationSystemId { get; set; }

    /// <summary>
    /// Gets or sets the information system.
    /// </summary>
    /// <value>The information system.</value>
    public virtual InformationSystem? InformationSystem { get; set; }

    /// <summary>
    /// Gets or sets the priority identifier.
    /// </summary>
    /// <value>The priority identifier.</value>
    public int? PriorityId { get; set; }

    /// <summary>
    /// Gets or sets the priority.
    /// </summary>
    /// <value>The priority.</value>
    public virtual Priority? Priority { get; set; }

    /// <summary>
    /// Gets or sets the task status identifier.
    /// </summary>
    /// <value>The task status identifier.</value>
    public int? TaskStatusId { get; set; }

    /// <summary>
    /// Gets or sets the task status.
    /// </summary>
    /// <value>The task status.</value>
    public virtual TaskStatus? TaskStatus { get; set; }

    /// <summary>
    /// Gets or sets the files.
    /// </summary>
    /// <value>The files.</value>
    public virtual List<File>? Files { get; set; }

    /// <summary>
    /// Gets or sets the task identifier.
    /// </summary>
    /// <value>The task identifier.</value>
    public int? CatalogTaskID { get; set; }

    /// <summary>
    /// Gets or sets the original task.
    /// </summary>
    /// <value>The original task.</value>
    public virtual CatalogTask? OriginalTask { get; set; }

    /// <summary>
    /// Gets or sets the tasks.
    /// </summary>
    /// <value>The tasks.</value>
    [JsonIgnore]
    public virtual List<CatalogTask>? CatalogTasks { get; set; }
}