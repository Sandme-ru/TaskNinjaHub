using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;

namespace TaskNinjaHub.Persistence.DataSeeders;

public static class DataSeederTaskStatus
{
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatalogTaskStatus>().HasData(new CatalogTaskStatus { Id = 1, Name = "Awaiting execution" });
        modelBuilder.Entity<CatalogTaskStatus>().HasData(new CatalogTaskStatus { Id = 2, Name = "At work" });
        modelBuilder.Entity<CatalogTaskStatus>().HasData(new CatalogTaskStatus { Id = 3, Name = "Awaiting verification" });
        modelBuilder.Entity<CatalogTaskStatus>().HasData(new CatalogTaskStatus { Id = 4, Name = "Done" });

        return modelBuilder;
    }
}