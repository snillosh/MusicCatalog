using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using MusicCatalog.ApiClient;

namespace MusicCatalog.Web.Auth;

public sealed class JwtAuthenticationStateProvider(IAccessTokenStore tokenStore) : AuthenticationStateProvider
{

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenStore.GetAccessTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            return Anonymous();
        }

        var identity = CreateIdentityFromJwt(token);

        if (identity is null)
        {
            return Anonymous();
        }

        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticatedAsync(string token)
    {
        await tokenStore.SetAccessTokenAsync(token);

        var identity = CreateIdentityFromJwt(token);
        var user = identity is null
            ? new ClaimsPrincipal(new ClaimsIdentity())
            : new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
        Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        await tokenStore.ClearAsync();

        NotifyAuthenticationStateChanged(
        Task.FromResult(Anonymous()));
    }

    private static AuthenticationState Anonymous()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        return new AuthenticationState(anonymousUser);
    }

    private static ClaimsIdentity? CreateIdentityFromJwt(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            if (jwt.ValidTo < DateTime.UtcNow)
            {
                return null;
            }

            var nameClaimType =
                jwt.Claims.Any(c => c.Type == ClaimTypes.Name) ? ClaimTypes.Name :
                jwt.Claims.Any(c => c.Type == "name") ? "name" :
                jwt.Claims.Any(c => c.Type == "email") ? "email" :
                jwt.Claims.Any(c => c.Type == "unique_name") ? "unique_name" :
                jwt.Claims.Any(c => c.Type == "sub") ? "sub" :
                ClaimTypes.Name;

            return new ClaimsIdentity(
            jwt.Claims,
            "jwt",
            nameClaimType,
            ClaimTypes.Role);
        }
        catch
        {
            return null;
        }
    }
}
