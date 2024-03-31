using TaskNinjaHub.Application.Entities.Tasks.Domain;

namespace TaskNinjaHub.Application.Entities.Tasks.Dto;

public class CatalogTaskRequestModel
{
    public CatalogTask Task { get; set; }

    public bool IsUpdated { get; set; }
}