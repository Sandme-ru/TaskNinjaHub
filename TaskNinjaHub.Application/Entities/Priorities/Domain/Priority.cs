using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.Priorities.Domain;

public class Priority : IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }
}