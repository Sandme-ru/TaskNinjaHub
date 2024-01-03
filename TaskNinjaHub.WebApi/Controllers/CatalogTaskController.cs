using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class CatalogTaskController : BaseController<CatalogTask, ITaskRepository>
{
    public CatalogTaskController(ITaskRepository repository) : base(repository)
    {

    }
}