using System.Text.Json.Serialization;
using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.Application.Entities.Tasks.Domain;

public class CatalogTask : BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? TaskAuthorId { get; set; }

    public virtual Author? TaskAuthor { get; set; }

    public int? TaskExecutorId { get; set; }

    public virtual Author? TaskExecutor { get; set; }

    public int? InformationSystemId { get; set; }

    public virtual InformationSystem? InformationSystem { get; set; }

    public int? PriorityId { get; set; }

    public virtual Priority? Priority { get; set; }

    public int? TaskStatusId { get; set; }

    public virtual TaskStatus? TaskStatus { get; set; }

    public virtual List<File>? Files { get; set; }

    public int? OriginalTaskId { get; set; }

    public virtual CatalogTask? OriginalTask { get; set; }
}