using Microsoft.AspNetCore.Components;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Components;

public partial class Header
{
    [Inject] 
    private IUserProviderService UserProviderService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    private string UserName => $"{UserProviderService.User.ShortName ?? "Anonymous"} ({UserProviderService.User.RoleName ?? "Anonymous"})";

    private void ShowUserProfile()
    {
        NavigationManager.NavigateTo("/"); //todo: Add user profile
    }

    private void RedirectBack()
    {
        NavigationManager.NavigateTo("/", true);
    }
}