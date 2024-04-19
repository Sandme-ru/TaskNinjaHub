using System.ComponentModel.DataAnnotations;
using TaskNinjaHub.Application.Attributes.ValidationAttributes;
using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.Application.Entities.Tasks.Domain;

public class CatalogTask : BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required field")]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Task author is required field")]
    public int TaskAuthorId { get; set; }

    public virtual Author? TaskAuthor { get; set; }

    [NotZero(ErrorMessage = "Task executor is required field")]
    public int TaskExecutorId { get; set; }

    public virtual Author? TaskExecutor { get; set; }

    [NotZero(ErrorMessage = "Information system is required field")]
    public int InformationSystemId { get; set; }

    public virtual InformationSystem? InformationSystem { get; set; }

    [NotZero(ErrorMessage = "Priority is required field")]
    public int PriorityId { get; set; }

    public virtual Priority? Priority { get; set; }

    [NotZero(ErrorMessage = "Task status is required field")]
    public int TaskStatusId { get; set; }

    public virtual CatalogTaskStatus? TaskStatus { get; set; }

    public virtual List<File>? Files { get; set; }

    public int? OriginalTaskId { get; set; }

    public virtual CatalogTask? OriginalTask { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    [NotZero(ErrorMessage = "Task type is required field")]
    public int TaskTypeId { get; set; }

    public virtual CatalogTaskType? TaskType { get; set; }
}