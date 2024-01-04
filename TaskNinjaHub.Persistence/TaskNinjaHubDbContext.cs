using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Persistence.DataSeeders;
using TaskNinjaHub.Persistence.EntityTypeConfigurations;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.Persistence;

public class TaskNinjaHubDbContext : DbContext, ITaskNinjaHubDbContext
{
    public TaskNinjaHubDbContext(DbContextOptions<TaskNinjaHubDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfiguration());

        modelBuilder = DataSeederInformationSystem.SeedData(modelBuilder);
        modelBuilder = DataSeederPriority.SeedData(modelBuilder);
        modelBuilder = DataSeederTaskStatus.SeedData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder
            .ConfigureWarnings(x => x.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
    }

    #region ENTITIES

    public DbSet<CatalogTask> CatalogTasks { get; set; } = null!;

    public DbSet<Author> Authors { get; set; } = null!;

    public DbSet<InformationSystem> InformationSystems { get; set; } = null!;

    public DbSet<Priority> Priorities { get; set; } = null!;

    public DbSet<TaskStatus> TaskStatuses { get; set; } = null!;

    public DbSet<File> Files { get; set; } = null!;

    #endregion
}