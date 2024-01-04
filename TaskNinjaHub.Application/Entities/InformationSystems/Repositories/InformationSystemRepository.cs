using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.InformationSystems.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.InformationSystems.Repositories;

public class InformationSystemRepository: BaseRepository<Domain.InformationSystem>, IInformationSystemRepository
{
    public InformationSystemRepository(ITaskNinjaHubDbContext? context) : base((DbContext)context!)
    {

    }
}