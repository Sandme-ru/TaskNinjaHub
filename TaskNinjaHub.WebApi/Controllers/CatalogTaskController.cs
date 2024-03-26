using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Dto;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Utilities.OperationResults;
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

    [HttpPost("CreateSameTask")]
    public async Task<OperationResult<CatalogTask>?> CreateSameTask([FromBody] CatalogTaskRequestModel? entity)
    {
        if (entity?.Task == null)
            return OperationResult<CatalogTask>.FailedResult("An empty object was passed to add");

        var result = await repository.CreateSameTask(entity.Task, entity.IsUpdated);
        return result;
    }
}