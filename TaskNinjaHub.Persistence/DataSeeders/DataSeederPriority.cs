using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Priorities.Domain;

namespace TaskNinjaHub.Persistence.DataSeeders;

public static class DataSeederPriority
{
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Priority>().HasData(new Priority { Id = 1, Name = "The highest" });
        modelBuilder.Entity<Priority>().HasData(new Priority { Id = 2, Name = "High" });
        modelBuilder.Entity<Priority>().HasData(new Priority { Id = 3, Name = "Medium" });
        modelBuilder.Entity<Priority>().HasData(new Priority { Id = 4, Name = "Low" });

        return modelBuilder;
    }
}