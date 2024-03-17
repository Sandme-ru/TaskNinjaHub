using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TaskNinjaHub.WebClient.Data;

public class CookieEvents(IUserTokenStore store) : CookieAuthenticationEvents
{
    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var token = await store.GetTokenAsync(context.Principal!);
        if (token.IsError)
        {
            context.RejectPrincipal();
        }

        await base.ValidatePrincipal(context);
    }

    public override async Task SigningOut(CookieSigningOutContext context)
    {
        await store.ClearTokenAsync(context.HttpContext.User);
        await base.SigningOut(context);
    }
}