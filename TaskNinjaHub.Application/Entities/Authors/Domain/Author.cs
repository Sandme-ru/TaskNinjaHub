using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Entities.Authors.Enums;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.Authors.Domain;

public class Author: BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ShortName { get; set; }

    public string? RoleName { get; set; }

    public string? AuthGuid { get; set; }

    [JsonIgnore]
    public virtual List<CatalogTask>? ExecutableTasks { get; set; }

    [JsonIgnore]
    public virtual List<CatalogTask>? AssignedTasks { get; set; }

    public LocalizationType? LocalizationType { get; set; }

    [NotMapped]
    public int? CountPerformedTasks { get; set; }

    [NotMapped] 
    public string? FullName => $"{ShortName} [{CountPerformedTasks}]";
}