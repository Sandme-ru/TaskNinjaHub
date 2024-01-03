using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class PriorityController : BaseController<Priority, IPriorityRepository>
{
    public PriorityController(IPriorityRepository repository) : base(repository)
    {

    }
}