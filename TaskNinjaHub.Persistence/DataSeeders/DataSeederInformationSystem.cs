using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;

namespace TaskNinjaHub.Persistence.DataSeeders;

public static class DataSeederInformationSystem
{
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InformationSystem>().HasData(new InformationSystem { Id = 1, Name = "The main information system" });

        return modelBuilder;
    }
}