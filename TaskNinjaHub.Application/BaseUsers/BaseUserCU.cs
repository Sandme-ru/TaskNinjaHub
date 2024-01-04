using System.ComponentModel.DataAnnotations;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.BaseUsers;

public class BaseUserCU : IHaveDateCreated
{
    private string? _userCreated;

    private string? _userUpdated;

    [Display(Name = "Автор записи")]
    [MaxLength(500)]
    public virtual string? UserCreated
    {
        get => _userCreated ?? "";
        set => _userCreated = value;
    }

    [Display(Name = "Автор изменения")]
    [MaxLength(500)]
    public virtual string? UserUpdated
    {
        get => _userUpdated ?? "";
        set => _userUpdated = value;
    }

    public virtual DateTime? DateCreated { get; set; }

    public virtual DateTime? DateUpdated { get; set; }

    public BaseUserCuData? Created => new()
    {
        Date = DateCreated, 
        User = UserCreated
    };

    public BaseUserCuData? Updated => new()
    {
        Date = DateUpdated, 
        User = UserUpdated
    };
}