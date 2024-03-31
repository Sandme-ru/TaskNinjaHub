using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.TaskTypes.Domain;
using TaskNinjaHub.Application.Entities.TaskTypes.Enum;

namespace TaskNinjaHub.Persistence.DataSeeders;

public static class DataSeederTaskType
{
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatalogTaskType>().HasData(new CatalogTaskType { Id = (int)EnumTaskType.Bug, Name = "Bug", DateCreated = DateTime.Now });
        modelBuilder.Entity<CatalogTaskType>().HasData(new CatalogTaskType { Id = (int)EnumTaskType.Feature, Name = "Feature", DateCreated = DateTime.Now });
        modelBuilder.Entity<CatalogTaskType>().HasData(new CatalogTaskType { Id = (int)EnumTaskType.Epic, Name = "Epic", DateCreated = DateTime.Now });
        modelBuilder.Entity<CatalogTaskType>().HasData(new CatalogTaskType { Id = (int)EnumTaskType.Testing, Name = "Testing", DateCreated = DateTime.Now });
        modelBuilder.Entity<CatalogTaskType>().HasData(new CatalogTaskType { Id = (int)EnumTaskType.Task, Name = "Task", DateCreated = DateTime.Now });
        modelBuilder.Entity<CatalogTaskType>().HasData(new CatalogTaskType { Id = (int)EnumTaskType.Requirement, Name = "Requirement", DateCreated = DateTime.Now });

        return modelBuilder;
    }
}