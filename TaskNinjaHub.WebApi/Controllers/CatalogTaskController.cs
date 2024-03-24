using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class CatalogTaskController(ITaskRepository repository) : BaseController<CatalogTask, ITaskRepository>(repository)
{
    public override async Task<int> GetAllCount()
    {
        var count = (await repository.GetAllAsync() ?? [])
        .Where(task => task.OriginalTaskId == null)
            .ToList()
            .Count;
        return count;
    }
}