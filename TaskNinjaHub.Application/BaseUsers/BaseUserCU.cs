using System.ComponentModel.DataAnnotations;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.BaseUsers;

/// <summary>
/// Class BaseUserCU.
/// Implements the <see cref="IHaveDateCreated" />
/// </summary>
/// <seealso cref="IHaveDateCreated" />
public class BaseUserCU : IHaveDateCreated
{
    /// <summary>
    /// The user created
    /// </summary>
    private string? _userCreated;

    /// <summary>
    /// The user updated
    /// </summary>
    private string? _userUpdated;

    /// <summary>
    /// Gets or sets the user created.
    /// </summary>
    /// <value>The user created.</value>
    [Display(Name = "Автор записи")]
    [MaxLength(500)]
    public virtual string? UserCreated
    {
        get => _userCreated ?? "";
        set => _userCreated = value;
    }

    /// <summary>
    /// Gets or sets the user updated.
    /// </summary>
    /// <value>The user updated.</value>
    [Display(Name = "Автор изменения")]
    [MaxLength(500)]
    public virtual string? UserUpdated
    {
        get => _userUpdated ?? "";
        set => _userUpdated = value;
    }

    /// <summary>
    /// Gets or sets the date created.
    /// </summary>
    /// <value>The date created.</value>
    public virtual DateTime? DateCreated { get; set; }

    /// <summary>
    /// Gets or sets the date updated.
    /// </summary>
    /// <value>The date updated.</value>
    public virtual DateTime? DateUpdated { get; set; }

    /// <summary>
    /// Gets the created.
    /// </summary>
    /// <value>The created.</value>
    public BaseUserCuData? Created => new()
    {
        Date = DateCreated, 
        User = UserCreated
    };

    /// <summary>
    /// Gets the updated.
    /// </summary>
    /// <value>The updated.</value>
    public BaseUserCuData? Updated => new()
    {
        Date = DateUpdated, 
        User = UserUpdated
    };
}