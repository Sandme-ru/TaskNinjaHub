using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.TaskStatuses.Domain;

public class CatalogTaskStatus: BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }
}