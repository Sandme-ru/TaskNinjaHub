using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Users.Domain;

namespace TaskNinjaHub.Persistence.DataSeeders;

/// <summary>
/// Class DataSeederUser.
/// </summary>
public static class DataSeederUser
{
    /// <summary>
    /// Seeds the data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <returns>ModelBuilder.</returns>
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "user1", Password = "user1", AuthorId = 1 });
        modelBuilder.Entity<User>().HasData(new User { Id = 2, Username = "user2", Password = "user2", AuthorId = 2 });
        modelBuilder.Entity<User>().HasData(new User { Id = 3, Username = "user3", Password = "user3", AuthorId = 3 });
        modelBuilder.Entity<User>().HasData(new User { Id = 4, Username = "user4", Password = "user4", AuthorId = 4 });
        modelBuilder.Entity<User>().HasData(new User { Id = 5, Username = "user5", Password = "user5", AuthorId = 5 });
        modelBuilder.Entity<User>().HasData(new User { Id = 6, Username = "user6", Password = "user6", AuthorId = 6 });
        modelBuilder.Entity<User>().HasData(new User { Id = 7, Username = "user7", Password = "user7", AuthorId = 7 });
        modelBuilder.Entity<User>().HasData(new User { Id = 8, Username = "user8", Password = "user8", AuthorId = 8 });

        return modelBuilder;
    }
}