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
        Response.Cookies.Delete(".ASPXAUTH_EDISON_ROLE");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, AuthProps());
    }

    private AuthenticationProperties AuthProps() => new()
    {
        RedirectUri = Url.Content("~/")
    };
}