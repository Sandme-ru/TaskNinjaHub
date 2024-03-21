using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Priorities.Repositories;

public class PriorityRepository(ITaskNinjaHubDbContext context) : BaseRepository<Priority>((DbContext)context), IPriorityRepository;