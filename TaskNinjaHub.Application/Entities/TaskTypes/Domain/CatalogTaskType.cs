using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.TaskTypes.Domain;

public class CatalogTaskType: BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }
}