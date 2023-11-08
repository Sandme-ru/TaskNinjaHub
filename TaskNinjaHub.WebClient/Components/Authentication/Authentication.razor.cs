using Microsoft.AspNetCore.Components;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.WebClient.Data.Authentication;

namespace TaskNinjaHub.WebClient.Components.Authentication;

/// <summary>
/// Class Authentication.
/// Implements the <see cref="ComponentBase" />
/// </summary>
/// <seealso cref="ComponentBase" />
public partial class Authentication
{
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="Authentication" /> is visibility.
    /// </summary>
    /// <value><c>true</c> if visibility; otherwise, <c>false</c>.</value>
    [Parameter]
    public bool Visibility { get; set; }

    /// <summary>
    /// Gets or sets the visibility changed.
    /// </summary>
    /// <value>The visibility changed.</value>
    [Parameter]
    public EventCallback<bool> VisibilityChanged { get; set; }

    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    /// <value>The navigation manager.</value>
    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    /// <summary>
    /// Gets or sets the website authenticator.
    /// </summary>
    /// <value>The website authenticator.</value>
    [Inject]
    private WebsiteAuthenticator? WebsiteAuthenticator { get; set; }

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>The user.</value>
    private User User { get; set; } = new();

    /// <summary>
    /// Tries the login.
    /// </summary>
    private async Task TryLogin()
    {
        await WebsiteAuthenticator?.LoginAsync(User)!;
        await VisibilityChanged.InvokeAsync(false);
        StateHasChanged();
    }

    /// <summary>
    /// Tries the logout.
    /// </summary>
    private async Task TryLogout()
    {
        await WebsiteAuthenticator?.LogoutAsync()!;
        await VisibilityChanged.InvokeAsync(false);
        StateHasChanged();
    }

    /// <summary>
    /// Handles the cancel.
    /// </summary>
    private void HandleCancel()
    {
        VisibilityChanged.InvokeAsync(false);
    }
}