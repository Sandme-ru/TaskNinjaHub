using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using TaskNinjaHub.WebClient.Services;

namespace TaskNinjaHub.WebClient.Components;

/// <summary>
/// Class Header.
/// Implements the <see cref="Microsoft.AspNetCore.Components.ComponentBase" />
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
public partial class Header
{
    /// <summary>
    /// Gets or sets a value indicating whether [authentication visibility].
    /// </summary>
    /// <value><c>true</c> if [authentication visibility]; otherwise, <c>false</c>.</value>
    private bool AuthenticationVisibility { get; set; } = false;

    [Inject] 
    private IUserProviderService UserProviderService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    /// <value>The navigation manager.</value>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IHttpContextAccessor HttpContextAccessor { get; set; } = null!;

    /// <summary>
    /// Gets the name of the user authentication.
    /// </summary>
    /// <value>The name of the user authentication.</value>
    private string UserAuthenticationName => $"{UserProviderService.UserName ?? "Anonymous"} ({UserProviderService.UserName ?? "Anonymous"})";

    /// <summary>
    /// Logins the handler.
    /// </summary>
    private void LoginHandler()
    {
        AuthenticationVisibility = true;
    }


    /// <summary>
    /// Shows the user profile.
    /// </summary>
    private async Task ShowUserProfile()
    {
        NavigationManager.NavigateTo("/userprofile");
    }

    /// <summary>
    /// Redirects the back.
    /// </summary>
    private async Task RedirectBack()
    {
        NavigationManager.NavigateTo("/", true);
    }
}