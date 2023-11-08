using Microsoft.AspNetCore.Components;
using TaskNinjaHub.WebClient.Data.Authentication;
using TaskNinjaHub.WebClient.Services.Bases;

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

    /// <summary>
    /// Gets or sets the user authentication service.
    /// </summary>
    /// <value>The user authentication service.</value>
    [Inject]
    private IUserAuthenticationService UserAuthenticationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the website authenticator.
    /// </summary>
    /// <value>The website authenticator.</value>
    [Inject]
    private WebsiteAuthenticator? WebsiteAuthenticator { get; set; }

    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    /// <value>The navigation manager.</value>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets the name of the user authentication.
    /// </summary>
    /// <value>The name of the user authentication.</value>
    private string UserAuthenticationName => $"{UserAuthenticationService.AuthorizedUser?.Username ?? "Anonymous"} ({UserAuthenticationService.AuthorizedUser?.Author?.Role?.Name ?? "Anonymous"})";

    /// <summary>
    /// Logins the handler.
    /// </summary>
    private void LoginHandler()
    {
        AuthenticationVisibility = true;
    }

    /// <summary>
    /// Tries the logout.
    /// </summary>
    private async Task TryLogout()
    {
        await WebsiteAuthenticator?.LogoutAsync()!;
        StateHasChanged();
        NavigationManager.NavigateTo("/");
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