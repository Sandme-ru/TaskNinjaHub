using Microsoft.AspNetCore.Components.Authorization;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

public class UserProviderService(AuthenticationStateProvider stateProvider, AuthorService authorService)
    : IUserProviderService
{
    public Author User { get; set; } = new();

    public async Task GetUser()
    {
        var authenticationState = await stateProvider.GetAuthenticationStateAsync();

        var shortName = authenticationState.User.FindFirst("short_name")?.Value;
        var roleName = authenticationState.User?.FindFirst("role_name")?.Value;
        var emailValue = authenticationState.User?.FindFirst("email_value")?.Value;

        User = new Author
        {
            Name = emailValue ?? string.Empty,
            RoleName = roleName,
            ShortName = shortName
        };

        Author? user = null;

        try
        {
            user = (await authorService.GetAllByFilterAsync(User))!
                .FirstOrDefault()!;
        }
        catch
        {
            // ignored
        }

        if (user != null)
        {
            User = user;
        }
    }
}