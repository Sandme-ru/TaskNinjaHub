using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using TaskNinjaHub.Application.Entities.TaskTypes.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.TaskTypes.Repositories;

public class TaskTypeRepository(ITaskNinjaHubDbContext context) : BaseRepository<CatalogTaskType>((DbContext)context), ITaskTypeRepository;