using Microsoft.AspNetCore.Components.Authorization;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Authors.Enums;
using TaskNinjaHub.WebClient.Services.Bases;
using TaskNinjaHub.WebClient.Services.HttpClientServices;

namespace TaskNinjaHub.WebClient.Services.UserProviders;

public class UserProviderService(AuthenticationStateProvider stateProvider, AuthorService authorService) : IUserProviderService
{
    public Author User { get; set; } = new();

    public async Task GetUser()
    {
        var authenticationState = await stateProvider.GetAuthenticationStateAsync();

        var shortName = authenticationState.User.FindFirst("short_name")?.Value;
        var roleName = authenticationState.User.FindFirst("role_name")?.Value;
        var emailValue = authenticationState.User.FindFirst("email_value")?.Value;
        var id = authenticationState.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        var localizationType = authenticationState.User.FindFirst("localization")?.Value;

        User = new Author
        {
            Name = emailValue ?? string.Empty,
            RoleName = roleName,
            ShortName = shortName,
            AuthGuid = id,
            LocalizationType = (LocalizationType)Enum.Parse(typeof(LocalizationType), localizationType ?? LocalizationType.None.ToString())
        };

        Author? user = null;

        try
        {
            user = (await authorService.GetAllByFilterAsync(User)).FirstOrDefault()!;
        }
        catch
        {
            // ignored
        }

        if (user != null)
            User = user;
    }
}