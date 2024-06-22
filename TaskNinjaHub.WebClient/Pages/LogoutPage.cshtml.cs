using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenIddict.Server.AspNetCore;

namespace TaskNinjaHub.WebClient.Pages;

public class LogoutModel : PageModel
{
    public async Task OnGetAsync()
    {
        Response.Cookies.Delete(".ASPXAUTH_EDISON_USERNAME");
        Response.Cookies.Delete(".ASPXAUTH_EDISON");
        Response.Cookies.Delete(".AspNetCore.Identity.Application");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, AuthProps());
    }

    private AuthenticationProperties AuthProps() => new()
    {
        RedirectUri = Url.Content("https://auth.sandme.ru/")
    };
    private AuthenticationProperties SignProps() => new()
    {
        RedirectUri = Url.Content("~/")
    };
}