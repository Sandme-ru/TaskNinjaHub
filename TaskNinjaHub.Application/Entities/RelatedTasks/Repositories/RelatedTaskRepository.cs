using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.RelatedTasks.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.RelatedTasks.Repositories;

public class RelatedTaskRepository(ITaskNinjaHubDbContext context) : BaseRepository<RelatedTask>((DbContext)context), IRelatedTaskRepository;