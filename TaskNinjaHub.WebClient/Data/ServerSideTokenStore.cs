using Duende.AccessTokenManagement.OpenIdConnect;
using IdentityModel;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace TaskNinjaHub.WebClient.Data;

public class ServerSideTokenStore : IUserTokenStore
{
    private readonly ConcurrentDictionary<string, UserToken> _tokens = new();

    public Task<UserToken> GetTokenAsync(ClaimsPrincipal user, UserTokenRequestParameters? parameters = null)
    {
        var sub = user.FindFirst(JwtClaimTypes.Subject)?.Value ?? throw new InvalidOperationException($"no {JwtClaimTypes.Subject} claim");

        if (_tokens.TryGetValue(sub, out var value))
        {
            if (string.IsNullOrWhiteSpace(value.RefreshToken))
                return Task.FromResult(new UserToken { Error = "not found refresh token" });

            return Task.FromResult(value);
        }

        return Task.FromResult(new UserToken { Error = "not found" });
    }

    public Task StoreTokenAsync(ClaimsPrincipal user, UserToken token, UserTokenRequestParameters? parameters = null)
    {
        var sub = user.FindFirst(JwtClaimTypes.Subject)?.Value ?? throw new InvalidOperationException($"no {JwtClaimTypes.Subject} claim");
        _tokens[sub] = token;

        return Task.CompletedTask;
    }

    public Task ClearTokenAsync(ClaimsPrincipal user, UserTokenRequestParameters? parameters = null)
    {
        var sub = user.FindFirst(JwtClaimTypes.Subject)?.Value ?? throw new InvalidOperationException($"no {JwtClaimTypes.Subject} claim");

        _tokens.TryRemove(sub, out _);
        return Task.CompletedTask;
    }
}