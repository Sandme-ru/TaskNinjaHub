﻿using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TaskNinjaHub.WebClient.Data;

public class CookieEvents : CookieAuthenticationEvents
{
    private readonly IUserTokenStore _store;

    public CookieEvents(IUserTokenStore store)
    {
        _store = store;
    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var token = await _store.GetTokenAsync(context.Principal!);
        if (token.IsError)
        {
            context.RejectPrincipal();
        }

        await base.ValidatePrincipal(context);
    }

    public override async Task SigningOut(CookieSigningOutContext context)
    {
        await _store.ClearTokenAsync(context.HttpContext.User);
        await base.SigningOut(context);
    }
}