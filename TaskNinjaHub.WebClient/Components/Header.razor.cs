using Microsoft.AspNetCore.Components;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Components;

public partial class Header
{
    [Inject] 
    private IUserProviderService UserProviderService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    private string UserName { get; set; } = null!;

    private bool IsLoading { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            IsLoading = true;
            StateHasChanged();

            UserName = $"{UserProviderService.User.ShortName ?? Localizer["Anonymous"].Value} ({UserProviderService.User.RoleName ?? Localizer["Anonymous"].Value})";

            IsLoading = false;
            StateHasChanged();
        }
    }

    private void ShowUserProfile()
    {
        NavigationManager.NavigateTo("/profile");
    }

    private void RedirectBack()
    {
        NavigationManager.NavigateTo("/", true);
    }
}