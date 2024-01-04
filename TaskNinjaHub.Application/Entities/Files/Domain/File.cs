using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.Files.Domain;

public class File : IHaveId, IHaveName, IHaveDateCreated
{
    public int Id { get; set; }
    
    public string? Name { get; set; }

    public string? Path { get; set; }

    public DateTime? DateCreated { get; set; }
    
    public int? TaskId { get; set; }

    public virtual CatalogTask? Task { get; set; }
}