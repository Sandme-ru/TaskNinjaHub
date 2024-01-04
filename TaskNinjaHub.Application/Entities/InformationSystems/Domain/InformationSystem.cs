using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.InformationSystems.Domain;

public class InformationSystem : BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }
}