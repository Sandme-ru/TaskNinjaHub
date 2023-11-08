using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Authors.Domain;

namespace TaskNinjaHub.Persistence.DataSeeders;

/// <summary>
/// Class DataSeederAuthor.
/// </summary>
public static class DataSeederAuthor
{
    /// <summary>
    /// Seeds the data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>ModelBuilder.</returns>
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData(new Author { Id = 1, Name = "First developer", RoleId = 1});
        modelBuilder.Entity<Author>().HasData(new Author { Id = 2, Name = "Second developer", RoleId = 1 });
        modelBuilder.Entity<Author>().HasData(new Author { Id = 3, Name = "First analyst", RoleId = 2 });
        modelBuilder.Entity<Author>().HasData(new Author { Id = 4, Name = "Second analyst", RoleId = 2 });
        modelBuilder.Entity<Author>().HasData(new Author { Id = 5, Name = "First support", RoleId = 3 });
        modelBuilder.Entity<Author>().HasData(new Author { Id = 6, Name = "Second support", RoleId = 3 });
        modelBuilder.Entity<Author>().HasData(new Author { Id = 7, Name = "First tester", RoleId = 4 });
        modelBuilder.Entity<Author>().HasData(new Author { Id = 8, Name = "Second tester", RoleId = 4 });

        return modelBuilder;
    }
}