using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.Application.Interfaces;

public interface ITaskNinjaHubDbContext
{
    DbSet<Author> Authors { get; set; }

    DbSet<InformationSystem> InformationSystems { get; set; }

    DbSet<Priority> Priorities { get; set; }

    DbSet<CatalogTask> CatalogTasks { get; set; }

    DbSet<TaskStatus> TaskStatuses { get; set; }
}