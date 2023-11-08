using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;

namespace TaskNinjaHub.Persistence.DataSeeders;

/// <summary>
/// Class DataSeederInformationSystem.
/// </summary>
public static class DataSeederInformationSystem
{
    /// <summary>
    /// Seeds the data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>ModelBuilder.</returns>
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InformationSystem>().HasData(new InformationSystem { Id = 1, Name = "The main information system" });

        return modelBuilder;
    }
}