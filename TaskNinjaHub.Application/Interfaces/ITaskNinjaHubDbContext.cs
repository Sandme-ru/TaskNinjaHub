using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.Application.Interfaces;

public interface ITaskNinjaHubDbContext
{
    DbSet<Author> Authors { get; set; }

    DbSet<InformationSystem> InformationSystems { get; set; }

    DbSet<Priority> Priorities { get; set; }

    DbSet<CatalogTask> CatalogTasks { get; set; }

    DbSet<CatalogTaskStatus> TaskStatuses { get; set; }

    DbSet<RelatedTask> RelatedTasks { get; set; }

    DbSet<File> Files { get; set; }

    DbSet<CatalogTaskType> TaskTypes { get; set; }
}