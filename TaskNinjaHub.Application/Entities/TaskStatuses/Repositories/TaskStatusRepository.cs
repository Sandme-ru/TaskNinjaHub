using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.TaskStatuses.Repositories;

public class TaskStatusRepository(ITaskNinjaHubDbContext context) : BaseRepository<CatalogTaskStatus>((DbContext)context), ITaskStatusRepository;