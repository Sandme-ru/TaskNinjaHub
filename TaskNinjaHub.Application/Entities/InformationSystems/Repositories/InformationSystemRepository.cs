using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.InformationSystems.Repositories;

public class InformationSystemRepository(ITaskNinjaHubDbContext context) : BaseRepository<InformationSystem>((DbContext)context), IInformationSystemRepository;