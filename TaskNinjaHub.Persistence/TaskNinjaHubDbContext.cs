using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.RelatedTasks.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Persistence.DataSeeders;
using TaskNinjaHub.Persistence.EntityTypeConfigurations;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.Persistence;

public class TaskNinjaHubDbContext(DbContextOptions<TaskNinjaHubDbContext> options) : DbContext(options), ITaskNinjaHubDbContext
{
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

    public DbSet<CatalogTaskStatus> TaskStatuses { get; set; } = null!;

    public DbSet<RelatedTask> RelatedTasks { get; set; } = null!;

    public DbSet<File> Files { get; set; } = null!;

    #endregion

    public void MigrateDatabase()
    {
        Database.Migrate();
    }
}