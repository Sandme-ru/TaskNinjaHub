using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Roles.Domain;
using TaskNinjaHub.Application.Enums;

namespace TaskNinjaHub.Persistence.DataSeeders;

/// <summary>
/// Class DataSeederRole.
/// </summary>
public static class DataSeederRole
{
    /// <summary>
    /// Seeds the data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>ModelBuilder.</returns>
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = EnumUserRole.Developer.ToString() });
        modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = EnumUserRole.Analyst.ToString() });
        modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = EnumUserRole.Support.ToString() });
        modelBuilder.Entity<Role>().HasData(new Role { Id = 4, Name = EnumUserRole.Tester.ToString() });

        return modelBuilder;
    }
}