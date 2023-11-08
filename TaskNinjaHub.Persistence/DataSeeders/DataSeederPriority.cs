using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Priorities.Domain;

namespace TaskNinjaHub.Persistence.DataSeeders;

/// <summary>
/// Class DataSeederPriority.
/// </summary>
public static class DataSeederPriority
{
    /// <summary>
    /// Seeds the data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>ModelBuilder.</returns>
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Priority>().HasData(new Priority { Id = 1, Name = "The highest" });
        modelBuilder.Entity<Priority>().HasData(new Priority { Id = 2, Name = "High" });
        modelBuilder.Entity<Priority>().HasData(new Priority { Id = 3, Name = "Medium" });
        modelBuilder.Entity<Priority>().HasData(new Priority { Id = 4, Name = "Low" });

        return modelBuilder;
    }
}