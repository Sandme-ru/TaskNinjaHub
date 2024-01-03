using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Interfaces;
using TaskNinjaHub.WebApi.Controllers.Bases;

namespace TaskNinjaHub.WebApi.Controllers;

public class InformationSystemController : BaseController<InformationSystem, IInformationSystemRepository>
{
    public InformationSystemController(IInformationSystemRepository repository) : base(repository)
    {

    }
}