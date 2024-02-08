using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.Tasks.Repositories;

public class TaskRepository(ITaskNinjaHubDbContext? context) : BaseRepository<CatalogTask>((DbContext)context!), ITaskRepository;