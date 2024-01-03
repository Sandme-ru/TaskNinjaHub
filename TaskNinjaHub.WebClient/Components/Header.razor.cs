using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using TaskNinjaHub.WebClient.Services;

namespace TaskNinjaHub.WebClient.Components;

public partial class Header
{
    [Inject] 
    private IUserProviderService UserProviderService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    private string UserName => $"{UserProviderService.User.Name ?? "Anonymous"} ({UserProviderService.User.Name ?? "Anonymous"})";

    private void ShowUserProfile()
    {
        NavigationManager.NavigateTo("/"); //todo: Add user profile
    }

    private void RedirectBack()
    {
        NavigationManager.NavigateTo("/", true);
    }
}