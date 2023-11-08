using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.Users.Domain;

/// <summary>
/// Class User.
/// </summary>
public class User : IHaveId
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the avatar path.
    /// </summary>
    /// <value>The avatar path.</value>
    public string? AvatarPath { get; set; }

    /// <summary>
    /// Gets or sets the author identifier.
    /// </summary>
    /// <value>The author identifier.</value>
    public int AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the author.
    /// </summary>
    /// <value>The author.</value>
    public virtual Author? Author { get; set; }
}